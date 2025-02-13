using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AutoUpdate
{
    
    public partial class AutoUpdateWindow : Form
    {

        protected UpdateManager _updMngr = new UpdateManager();
        private int _initHeight;
        private int _initWidth;
        private int _initProgressBarWidth;

        public void SetUpdateUrl (string updateUrl)
        {
            _updMngr.UpdateUrl = updateUrl;
        }

        public void SetCurrentVersion (string ver)
        {
            _updMngr.CurrentVersion = ver;
        }

        public AutoUpdateWindow()
        {
            InitializeComponent();
            CropWindowFunc();
        }

        private void CropWindowFunc ()
        {
            _initProgressBarWidth = progressBar.Width;
            _initHeight = this.Height;
            this.Height = 105;
            webBrowser.Hide();
            btnInstall.Hide();
            _initWidth = this.Width;
            this.Width = 350;
            progressBar.Width = this.Width - 40;
        }

        private void RestoreWindowFunc()
        {
            this.Height = _initHeight;
            this.Width = _initWidth;
            progressBar.Width = _initProgressBarWidth;
            webBrowser.Show();
            btnInstall.Show();
        }

        private void DownloadStatusChange(UpdateStatus newStatus)
        {
            switch (newStatus)
            {
                case UpdateStatus.CheckingUpdate: lblStatus.Text = "Update checking ..."; break;
                case UpdateStatus.ReleaseNotesDownloading: lblStatus.Text = "Downloading release notes ..."; break;
                case UpdateStatus.ReleaseNotesDownloaded: RestoreWindowFunc(); progressBar.Style = ProgressBarStyle.Blocks; break;
                case UpdateStatus.UpdateNotFound: lblStatus.Text = "No updates found"; progressBar.Style = ProgressBarStyle.Blocks; progressBar.Step = 100; break;
                case UpdateStatus.UpdateFound: break;
                case UpdateStatus.UpdateDownloading: lblStatus.Text = "Downloading updates ..."; break;
                case UpdateStatus.Done: lblStatus.Text = "Done"; break;
                default: lblStatus.Text = "Done"; break;
            }
        }

        private void ProgressChange(decimal progress)
        {
            if (progress > -1)
            {
                progressBar.Value = (int)progress;
            }
        }

        private void ReleaseNotesDownloadedHandler(string filePath)
        {
            webBrowser.Navigate(filePath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _updMngr.DownloadUpdate();
        }

        private void AutoUpdateWindow_Shown(object sender, EventArgs e)
        {
            try
            {
                _updMngr.OnReleaseNotesDownloaded += ReleaseNotesDownloadedHandler;
                _updMngr.OnDownloadProgressChange += ProgressChange;
                _updMngr.OnStatusChange += DownloadStatusChange;
                _updMngr.OnError += (Exception ex) =>
                {
                    MessageBox.Show((IWin32Window)this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _updMngr.CleanDownloadedFiles();
                    this.Close();
                };
                var update = _updMngr.CheckForUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show((IWin32Window)this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
