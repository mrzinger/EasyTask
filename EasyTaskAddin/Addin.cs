using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NetOffice.Tools;
using Outlook = NetOffice.OutlookApi;
using NetOffice.OutlookApi.Enums;
using NetOffice.OutlookApi.Tools;
using Office = NetOffice.OfficeApi;
using NetOffice.OfficeApi.Tools;
using OutlookControlsExtensions;
using WinApi;
using System.Diagnostics;
using log4net;
using System.Xml;
using log4net.Config;

namespace EasyTaskAddin
{
    [COMAddin("EasyTask", "This add-in empowers standard Office tasks with possibility of using special keywords to specify different tasks attributes", 3), ProgId("EasyTask.Addin"),
        Guid("509DC54F-081E-410D-A096-A875AFA66FC0")]
	[RegistryLocation(RegistrySaveLocation.CurrentUser), CustomUI("EasyTaskAddin.RibbonUI.xml")]
	public partial class Addin : BaseRibbonHelper
	{
        Outlook.Items _taskItems = null;
        DueDateProcessor _dueProcessor = new DueDateProcessor();
        CategoryProcessor _categoryProcessor = new CategoryProcessor();
        SettingsHandler _settingsHandler = null;
        ProcessedTasksDatabase _tasksDb = null;

        #region OL Controls extensions
        FilteredListExtension _filteredList = new FilteredListExtension();
        TaskEditBoxExstensionsManager _tebeManager = null;
        #endregion

        private static readonly ILog log = LogManager.GetLogger("EasyTask");

        internal Office.IRibbonUI RibbonUI { get; private set; }

        public Addin()
		{
            this.OnStartupComplete += new OnStartupCompleteEventHandler(Addin_OnStartupComplete);
            this.OnDisconnection += new OnDisconnectionEventHandler(Addin_OnDisconnection);
            _settingsHandler = new SettingsHandler(_dueProcessor, _categoryProcessor, _filteredList);
        }

        private void Addin_OnStartupComplete(ref Array custom)
		{
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(Properties.Resources.loggerconfig);

                var col = XmlConfigurator.Configure((XmlElement)xmlDoc.FirstChild);
                log.Info("Logger is initialized");

                //Console.WriteLine("Addin started in Outlook Version {0}", Application.Version);
                _taskItems = (Outlook.Items)this.Application.Session.GetDefaultFolder(OlDefaultFolders.olFolderToDo).Items; //OL TODO?
                _taskItems.ItemAddEvent += new Outlook.Items_ItemAddEventHandler(TaskItems_ItemAdd);
                _taskItems.ItemChangeEvent += new Outlook.Items_ItemChangeEventHandler(TaskItems_ItemChange);

                _tasksDb = new ProcessedTasksDatabase(_taskItems);

                InitControlExtensions();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception :" + ex.Message);
            }
        }

        private void InitControlExtensions ()
        {
            _tebeManager = new TaskEditBoxExstensionsManager();
            
            InitKeywordsExtension(_filteredList);
            _tebeManager.AddExtension(_filteredList);

            IntPtr mainOlWindow = GetOutlookMainHWnd();

            if (mainOlWindow != IntPtr.Zero)
                _tebeManager.Start(mainOlWindow);
            else
                log.Debug ("Error finding OL main wnd"); /// TODO Replace with normal logging
        }

        public static IntPtr GetOutlookMainHWnd()
        {
            int curtProcId = Process.GetCurrentProcess().Id;
            int wndProcId = 0;
            IntPtr olMainHWnd = IntPtr.Zero;
            while (curtProcId != wndProcId)
            {
                olMainHWnd = NativeMethods.FindWindowEx(IntPtr.Zero, olMainHWnd, "rctrl_renwnd32", IntPtr.Zero);
                NativeMethods.GetWindowThreadProcessId(olMainHWnd, out wndProcId);
            }
            
            return olMainHWnd;
        }

        private void InitKeywordsExtension (FilteredListExtension flExt)
        {
            /// TODO add more smart way of identifying appropriate keyword
            var nameSpace = this.Application.Session;
            foreach (var category in nameSpace.Categories)
                flExt.Keywords.Add("[" + category.Name + "]");
        }   

        private void TaskItems_ItemAdd(object Item)
        {
            ProcessItem((Outlook.TaskItem)Item);
        }

        private void TaskItems_ItemChange(object Item)
        {
            if ((Properties.Settings.Default.ProcessChangedTask == true) && 
                (_tasksDb.CheckAndUpdate((Outlook.TaskItem)Item) == false))
                ProcessItem((Outlook.TaskItem)Item);
        }

        private void ProcessItem(Outlook.TaskItem Item)
        {
            if (Properties.Settings.Default.EnableAddin == false)
                return;

            var item = (Outlook.TaskItem)Item;
            item = _dueProcessor.ParseAndSetDueDate(item);
            item = _categoryProcessor.ParseAndSetCategory(item);
            if (item.Saved == false)
                item.Save();
        }

        private void Addin_OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
		{
		}

		protected override void OnError(ErrorMethodKind methodKind, System.Exception exception)
		{
			MessageBox.Show("An error occurend in " + methodKind.ToString(), "EasyTask");
		}

		[RegisterErrorHandler]
		public static void RegisterErrorHandler(RegisterErrorMethodKind methodKind, System.Exception exception)
		{
			MessageBox.Show("An error occurend in " + methodKind.ToString(), "EasyTask");
		}

    }
}

