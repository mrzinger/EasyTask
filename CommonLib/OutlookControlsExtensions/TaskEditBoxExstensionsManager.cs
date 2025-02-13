using log4net;
using ManagedWinapi.Windows;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WinApi;

namespace OutlookControlsExtensions
{
    public class TaskEditBoxExstensionsManager
    {
    

        private static readonly ILog log = LogManager.GetLogger(typeof(TaskEditBoxExstensionsManager));

        TaskEditBoxMonitor _monitor = null;

        List<ITaskEditBoxExtension> _extList = new List<ITaskEditBoxExtension>();

        public TaskEditBoxExstensionsManager ()
        {
            _monitor = TaskEditBoxMonitor.Instance;
            _monitor.OnEditBoxCreated += OnEditBoxCreated;
            _monitor.OnEditBoxDestroyed += OnEditBoxDestroyed;
            _monitor.OnTextChanged += OnTextChanged;
            _monitor.OnKeyPressed += OnKeyPressed;
        }

        public void Start (IntPtr outlookMainHwnd)
        {
            _monitor.StartMonitoring(outlookMainHwnd);
        }

        public void AddExtension (ITaskEditBoxExtension extension)
        {
            _extList.Add(extension); 
        }

        void OnTextChanged(SystemWindow triggerControl, char cChar)
        {
            try
            {
                string windowText = triggerControl.Title;

                /*if (String.IsNullOrEmpty (windowText))
                    return; 
                    */
                int carretPos = GetCursorPosition(triggerControl.HWnd);

                if (cChar == (char)Keys.Back) /// backspace
                {
                    log.Debug("Backspace key is pressed");
                    cChar = (char)0;
                    if (carretPos > 0)
                        carretPos--;
                }

                foreach (var extension in _extList)
                {

                    if (extension.OnTextChanged(triggerControl, windowText, TrimCurrentString(windowText.ToCharArray(), carretPos, cChar), carretPos) == true)
                    {
                        /// The extension has processed the message so no other extensions has to be called
                        break;
                    }
                }
            }catch (Exception ex)
            {
                log.ErrorFormat("Exception in OnTextChanged {0}", ex.Message);
            }
        }

        void OnEditBoxCreated(SystemWindow triggerControl)
        {
            foreach (var ext in _extList)
            {
                var childCtrl = ext.GetControl();
                childCtrl.Hide();
                NativeMethods.SetParent(childCtrl.Handle, _monitor.ContainerHwnd);
                
                var pos = triggerControl.Parent.Position;
                childCtrl.Left = pos.Left;
                childCtrl.Width = pos.Width;
                childCtrl.Top = pos.Top + pos.Height;

                ext.OnEditBoxCreated(triggerControl);
            }
        }

        void OnEditBoxDestroyed(SystemWindow triggerControl)
        {
            _extList.ForEach(r => { r.GetControl().Hide(); });
        }

        void OnKeyPressed(SystemWindow triggerControl, Keys key)
        {
            if (key == Keys.Down)
            {
                _extList.ForEach(r => { r.GetControl().Focus(); r.OnKeyPressed(triggerControl, key); });
            }
        }

        protected int GetCursorPosition(IntPtr hEdit)
        {
            try
            {
                log.Debug("GetCursorPosition");
                IntPtr lParam = IntPtr.Zero, wParam = IntPtr.Zero;
                NativeMethods.CHARRANGE caretPos = new NativeMethods.CHARRANGE
                { cpMin = 0, cpMax = 0};

                int result = (int)NativeMethods.SendMessage(hEdit, (uint)NativeMethods.EditMessages.EM_EXGETSEL, wParam, ref caretPos);
                
               // caretPos = (NativeMethods.CHARRANGE) Marshal.PtrToStructure(lParam, typeof(NativeMethods.CHARRANGE));
                //return result >> 16;
                return caretPos.cpMin;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
                return 0;
            }
        }
        
        /// <summary>
        /// Returns string from charsArray located in carretPos and insert curChar to it. 
        /// </summary>
        /// <param name="charsArray">Chars array that represents edit box text</param>
        /// <param name="carretPos">Current carret position</param>
        /// <param name="curChar">Char to be inserted to current carret position. If 0 then nothing is inserted</param>
        /// <returns></returns>
        protected CurrentStringParams TrimCurrentString(char[] charsArray, int carretPos, char curChar)
        {
            log.Debug("TrimCurrentString");
            StringBuilder sb = new StringBuilder();
            CurrentStringParams csParams;
            int index = 0;

            for (index = carretPos - 1; index > -1 && charsArray[index] != ' '; index--)
            {
                /// Back
            }

            csParams.StartPos = index;
            bool isAddtoEnd = true;

            for (index++; index < charsArray.Length && charsArray[index] != ' '; index++)
            {
                /// and forth
                if (index == carretPos && curChar > 0)
                {
                    sb.Append(curChar);
                    isAddtoEnd = false;
                }
                sb.Append(charsArray[index]);
            }

            if (isAddtoEnd == true && curChar > 0)
            {
                sb.Append(curChar);
                index++;
            }

            csParams.EndPos = index;
            csParams.CurText = sb.ToString();
            csParams.CarretPos = carretPos;
            log.Debug("TrimCurrentString end");
            return csParams;
        }
    }
}
