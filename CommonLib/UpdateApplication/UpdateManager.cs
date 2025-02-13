using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoUpdate
{
    public enum UpdateStatus
    {
        CheckingUpdate,
        UpdateNotFound,
        UpdateFound,
        ReleaseNotesDownloading,
        ReleaseNotesDownloaded,
        UpdateDownloading,
        UpdateDownloaded,
        Done
    };

    public class UpdateManager
    {
        private static readonly ILog log = LogManager.GetLogger("EasyTask");

        internal enum MoveFileFlags
        {
            MOVEFILE_REPLACE_EXISTING = 1,
            MOVEFILE_COPY_ALLOWED = 2,
            MOVEFILE_DELAY_UNTIL_REBOOT = 4,
            MOVEFILE_WRITE_THROUGH = 8
        }

        [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "MoveFileEx")]
        internal static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName,
        MoveFileFlags dwFlags);

        public class Channel
        {
            public struct ItemStruct
            {
                public string Version;
                public string ReleaseNotesUrl;
                public string PubDate;
                public string FileName;
                public string DownloadUrl;
            }
            public string Title;
            public string Description;
            public string Language;
            public ItemStruct Item;
        }


        private Channel _channel = null;

        public delegate void ReleaseNotesDownloadedEvent(string filePath);
        public event ReleaseNotesDownloadedEvent OnReleaseNotesDownloaded;

        public delegate void DonwloadProgressChangeEvent(decimal progress);
        public event DonwloadProgressChangeEvent OnDownloadProgressChange;

        public delegate void StatusChangeEvent(UpdateStatus newStatus);
        public event StatusChangeEvent OnStatusChange;

        public delegate void ErrorEvent (Exception ex);
        public event ErrorEvent OnError;

        protected string _updateUrl = string.Empty;

        public string UpdateUrl
        {
            set { _updateUrl = value; }
            get { return _updateUrl; }
        }

        protected string _currentVersion = string.Empty;

        public string CurrentVersion
        {
            set { _currentVersion = value; }
            get { return _currentVersion; }
        }

        private string _tempDirPath = string.Empty;


        public async Task CheckForUpdate()
        {
            try
            {
                if (UpdateUrl == string.Empty)
                    throw new Exception("Update url is not specified");

                var fileName = "address.xml";
                try
                {
                    _tempDirPath = Path.Combine (Path.GetTempPath(), "EatOUpdateFolder");
                    log.Debug ("Creating temp file directory");
                    if (Directory.Exists(_tempDirPath))
                        Directory.Delete(_tempDirPath, true);
                    Directory.CreateDirectory(_tempDirPath);
                    fileName = Path.Combine(_tempDirPath, fileName);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating/clearing the download directory.", ex);
                }

                using (WebClient wc = new WebClient())
                {
                    var lockThis = new object();

                    OnStatusChange(UpdateStatus.CheckingUpdate);

                    wc.DownloadProgressChanged += (dsender, eventArgs) =>
                    {
                        lock (lockThis)
                        {
                            OnDownloadProgressChange(eventArgs.BytesReceived / eventArgs.TotalBytesToReceive * 100);
                        }
                    };
                    await wc.DownloadFileTaskAsync(this.UpdateUrl, fileName);

                    _channel = DeSerializeChannel(fileName);

                    if (IsNewVeresion(_channel) == false)
                    {
                        OnStatusChange(UpdateStatus.UpdateNotFound);
                        return;
                    }

                    OnStatusChange(UpdateStatus.UpdateFound);

                    OnStatusChange(UpdateStatus.ReleaseNotesDownloading);
                    fileName = "releasenotes.htm";
                    fileName = Path.Combine(_tempDirPath, fileName);
                    await wc.DownloadFileTaskAsync(_channel.Item.ReleaseNotesUrl, fileName);

                    OnReleaseNotesDownloaded(fileName);
                    OnStatusChange(UpdateStatus.ReleaseNotesDownloaded);
                }
            }catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public async Task DownloadUpdate()
        {
            try
            {
                if (_channel == null)
                    throw new Exception("Update channel was not initialized run CheckForUpdate first");

                using (WebClient wc = new WebClient())
                {
                    var lockThis = new object();
                    var targetFile = GetUpdateFilePath(_channel);

                    OnStatusChange(UpdateStatus.UpdateDownloading);

                    wc.DownloadProgressChanged += (dsender, eventArgs) =>
                    {
                        lock (lockThis)
                        {
                            OnDownloadProgressChange(eventArgs.BytesReceived / eventArgs.TotalBytesToReceive * 100);
                        }
                    };
                    await wc.DownloadFileTaskAsync(_channel.Item.DownloadUrl, targetFile);
                    OnStatusChange(UpdateStatus.UpdateDownloaded);
                }
                LaunchUpdate();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public void LaunchUpdate()
        {
            var targetFile = GetUpdateFilePath(_channel);
            Process.Start(targetFile);
            CleanDownloadedFiles(true);
        }

        protected string GetUpdateFilePath (Channel channel)
        {
            return Path.Combine(_tempDirPath, string.Concat(channel.Title, channel.Item.Version, channel.Item.FileName));
        }

        protected Channel DeSerializeChannel(string filePath)
        {
            XmlSerializer myDeserializer = new XmlSerializer(typeof(Channel));
            using (var myFileStream = new FileStream(filePath, FileMode.Open))
            {
                return (Channel)myDeserializer.Deserialize(myFileStream);
            }
        }

        protected bool IsNewVeresion (Channel channel)
        {
            Version currentVersion = null;

            if (CurrentVersion == string.Empty)
                currentVersion = this.GetType().Assembly.GetName().Version;
            else
                currentVersion = Version.Parse(CurrentVersion);

            if (channel.Item.Version == null)
                throw new Exception("Version of the update is not found");

            return currentVersion < Version.Parse(channel.Item.Version);
        }

        public void CleanDownloadedFiles (bool isDelayed = false)
        {
            foreach (var fileName in Directory.EnumerateFiles(_tempDirPath))
            {
                if (isDelayed)
                    MoveFileEx(fileName, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
                else
                    File.Delete(fileName);
            }
            if (isDelayed)
                MoveFileEx(_tempDirPath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
            else
                Directory.Delete(_tempDirPath);
        }
    }
}
