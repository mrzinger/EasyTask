using System;
using System.Linq;
using System.Windows.Forms;
using ManagedWinapi.Windows;
using log4net;
using WinApi;
using System.Collections;
using System.Collections.Generic;

namespace OutlookControlsExtensions
{
    public class FilteredListExtension : ListBox, ITaskEditBoxExtension
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FilteredListExtension));

        public List<string>  Keywords = new List<string>();

        public char? FilterTriggerChar { get; set; } //what char should trigger filter list to show up

        SystemWindow _curEditBox = null;
        CurrentStringParams _csParams;

        private void Initialize()
        {
            this.SuspendLayout();
            // 
            // contentListBox
            // 
            this.FilterTriggerChar = null;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormattingEnabled = true;

            this.TabIndex = 0;
            // 
            // FilteredList
            // 
            this.Name = "FilteredList";
            this.Size = new System.Drawing.Size(0, 0);
            this.ResumeLayout(false);

        }

        string _filterText = string.Empty;

        public string FilterText
        {
            set
            {
                _filterText = value;
                this.Items.Clear();
                this.BeginUpdate();
                foreach (var item in  Keywords.Where(keyword => keyword.Contains(_filterText)))
                    this.Items.Add (item);
                this.EndUpdate();
                if (Items.Count > 0)
                {
                    Height = (Items.Count + 1) * ItemHeight;
                    if (Visible == false)
                        Show();
                }
                else
                    Hide();
                log.DebugFormat("Current filtered string {0} amount of keywords {1}. Control height is {2}", value, this.Items.Count, this.Height);
            }
            get
            {
                return _filterText;
            }
        }

        public FilteredListExtension()
        {
            Initialize();
        }

        public Control GetControl()
        {
            return this;
        }

        public bool OnTextChanged(SystemWindow triggerControl, string editBoxString, CurrentStringParams csParams, int carretPosition)
        {
            if (this.FilterTriggerChar != null && csParams.CurText[0] == this.FilterTriggerChar)
            {
                this.FilterText = csParams.CurText;
                _csParams = csParams;
            }
           return false;
        }

        public void OnEditBoxCreated(SystemWindow triggerControl)
        {
            _curEditBox = triggerControl;
        }

        public void OnEditBoxDestroyed(SystemWindow triggerControl)
        {
            this.Hide();
            _curEditBox = null;
        }

        public bool OnKeyPressed(SystemWindow triggerControl, Keys key)
        {
            if (Visible == true)
            {
                this.SetSelected(0, true);
                return true;
            }

            return false;
        }

        protected void SetSelectedValue()
        {
            /// We need to replace current text that is surrounded by spaces 

            var strProcessed = _curEditBox.Title.Remove(_csParams.StartPos + 1, this.FilterText.Length);
            strProcessed = strProcessed.Insert(_csParams.StartPos + 1, this.SelectedItem.ToString());
            _curEditBox.Title = strProcessed;

            NativeMethods.SetFocus(_curEditBox.HWnd);

            //Move carred after ] simbol to be able to contiue typing subject without interruptions
            var carretPos = _csParams.CarretPos + this.SelectedItem.ToString().Length + 1;
            NativeMethods.SetCarretPos(_curEditBox.HWnd, carretPos, carretPos);

            Hide();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Focus();
            SetSelectedValue();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
                SetSelectedValue();
        }

    }
}
