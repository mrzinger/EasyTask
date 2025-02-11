using NetOffice.OfficeApi;
using System;
using System.Diagnostics;
using System.Drawing;
using Office = NetOffice.OfficeApi;

namespace EasyTaskAddin
{
    public partial class Addin
    {
        public void OnLoadRibonUI(Office.IRibbonUI ribbonUI)
        {
            RibbonUI = ribbonUI;
        }

        public void OnUpdateButton (Office.IRibbonControl control)
        {
            AutoUpdate.AutoUpdateWindow updateWnd = new AutoUpdate.AutoUpdateWindow();
            updateWnd.SetUpdateUrl("https://onedrive.live.com/download?cid=93242D385A3DD794&resid=93242D385A3DD794%2115410&authkey=AFNHNTUuH-GYegI");
            updateWnd.SetCurrentVersion(this.GetType().Assembly.GetName().Version.ToString());
            updateWnd.ShowDialog();
        }

        public void OnSettingsButton(Office.IRibbonControl control)
        {
            Properties.Settings.Default.Reload();
            var settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.Save();
                _settingsHandler.UpdateSettings();
            }
        }

        public void OnEnableCheckbox(Office.IRibbonControl control, bool pressed)
        {
            Properties.Settings.Default.EnableAddin = pressed;
            Properties.Settings.Default.Save();
        }

        public bool OnPressCheckbox(Office.IRibbonControl control)
        {
            return Properties.Settings.Default.EnableAddin;
        }

        public void OnBugButton(IRibbonControl control)
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.Verb = "open";

            procInfo.FileName = string.Format("mailto:eou.bug@outlook.com?subject=[Bug {0}]%20Enter%20title%20please&body=Version {1}%0APlease enter defect description here",
                DateTime.Now.ToString(),
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Replace(" ", "%20");
            Process.Start(procInfo);
        }

        public void OnAboutButton(IRibbonControl control)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
        
    }
}
