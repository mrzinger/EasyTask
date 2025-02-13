using log4net;
using ManagedWinapi.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinApi;

namespace OutlookControlsExtensions
{
    class TaskEditBoxMonitor
    {
        public delegate void TextChanged(SystemWindow triggerControl, char cChar);
        public delegate void EditBoxCreated(SystemWindow triggerControl);
        public delegate void EditBoxDestroyed(SystemWindow triggerControl);
        public delegate void KeyPressed(SystemWindow triggerControl, Keys key);

        public event TextChanged OnTextChanged;
        public event EditBoxCreated OnEditBoxCreated;
        public event EditBoxDestroyed OnEditBoxDestroyed;
        public event KeyPressed OnKeyPressed;
        
        private static readonly ILog log = LogManager.GetLogger(typeof(TaskEditBoxMonitor));
        private static readonly ILog logMsg = LogManager.GetLogger("WindowsMessages");
        /// <summary>
        /// Subclass manager to attach ToDoView monitor and extend task text editor functionality
        /// </summary>
        protected WindowSubclassManager _scManager = new WindowSubclassManager();

        /// <summary>
        /// Application sole to have uniquie identifier for subcluss window proc
        /// </summary>
        private int _appSole = 4242;


        protected IntPtr _containerHwnd = IntPtr.Zero;

        public IntPtr ContainerHwnd
        {
            get
            {
                return _containerHwnd;
            }
        }

        protected IntPtr _editBoxHwnd = IntPtr.Zero;
        public IntPtr EditBoxHwnd
        {
            get
            {
                return _editBoxHwnd;
            }
        }

        #region Singletone implementation
        private static volatile TaskEditBoxMonitor instance;
        private static object syncRoot = new Object();

        public static TaskEditBoxMonitor Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TaskEditBoxMonitor();
                    }
                }

                return instance;
            }
        }

        private TaskEditBoxMonitor() // prohibit direct instanciation as we should have the only instance of it
        {

        }
        #endregion

        public bool StartMonitoring(IntPtr outlookMainHwnd)
        {
            if (outlookMainHwnd == IntPtr.Zero)
            {
                log.Error("Outlook main window handler is NULL");
                return false;
            }

            if (outlookMainHwnd != IntPtr.Zero)
            {
                try
                {
                    this._scManager.Reset();

                    log.DebugFormat("Main Outlook window handler 0x{0}", outlookMainHwnd.ToString("X"));
                    // Enumerate all Outlook windows and add monitoring handler
                    _containerHwnd = _scManager.FindAndSubclassWnd(outlookMainHwnd, "rctrl_renwnd32", "ToDoView", ContainerMonitor, _appSole);

                    if (_containerHwnd == IntPtr.Zero)
                    {
                        log.Error("Couldn't find ToDoView container window");
                        return false;
                    }

                    log.DebugFormat("Found ToDoView container window 0x{0}", _containerHwnd.ToString("X"));
                } catch (Exception ex)
                {
                    log.ErrorFormat("Exception in StartMonigoring function {0}", ex.Message);
                    return false;
                }
            }
            return true;
        }

        private IntPtr ContainerMonitor(IntPtr hwnd, IntPtr msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data)
        {
            try
            {
                logMsg.DebugFormat("TODO View Message: {0} with wParam: {1} (low 0x{2} high 0x{3}); lParam: 0x{4}",
                    ((NativeMethods.WindowsMessages)msg).ToString(), wParam.ToString(), ((short)wParam).ToString("X"), ((short)wParam >> 16).ToString("X"), lParam.ToString("X"));

                if (msg == (IntPtr)NativeMethods.WindowsMessages.WM_NOTIFY)
                {
                    NativeMethods.NMHDR structNMHDR = new NativeMethods.NMHDR();
                    Marshal.PtrToStructure(lParam, structNMHDR);
                    log.DebugFormat("WM_NOTIFY NMHDR struct hwndFrom: 0x{0}, idFrom: 0x{1}, code: {2}", structNMHDR.hwndFrom.ToString("X"), structNMHDR.idFrom.ToString("X"), structNMHDR.code);
                }

                if (msg == (IntPtr)NativeMethods.WindowsMessages.WM_NOTIFY &&
                    (int)wParam == 0) // Don't have good explanation but it looks like this event is sent when edit boxes appear
                {
                    log.DebugFormat("TODO View. New edit box is created. Start subclassing");
                    IntPtr editBoxHwnd = _scManager.FindAndSubclassWnd(hwnd, "RichEdit20WPT", null, EditBoxHandler, _appSole); //maybe add test if already control exist????


                    if (OnEditBoxCreated != null && editBoxHwnd != _editBoxHwnd)
                    {
                        OnEditBoxCreated(new SystemWindow(editBoxHwnd));
                        _editBoxHwnd = editBoxHwnd;
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Exception in ContainerMonitor function {0}", ex.Message);
            }

            return NativeMethods.DefSubclassProc(hwnd, msg, wParam, lParam);
        }

        private IntPtr EditBoxHandler(IntPtr hwnd, IntPtr msg, IntPtr wParam, IntPtr lParam, IntPtr id, IntPtr data)
        {
            try
            {
                logMsg.DebugFormat("EDIT Box Message: {0} with wParam: {1}; lParam: 0x{2}",
                    ((NativeMethods.WindowsMessages)msg).ToString(), wParam.ToString(), lParam.ToString("X"));
                switch ((NativeMethods.WindowsMessages)msg)
                {
                    case NativeMethods.WindowsMessages.WM_DESTROY:
                        {
                            if (_scManager.RemoveWindowSubclass(hwnd) > 0)
                            {
                                log.DebugFormat("Removed subclass for editbox window 0x{0}", hwnd.ToString("X"));
                                _editBoxHwnd = IntPtr.Zero;
                            }
                            if (OnEditBoxDestroyed != null)
                                OnEditBoxDestroyed(new SystemWindow(hwnd));
                            break;
                        }
                    case NativeMethods.WindowsMessages.WM_CHAR:
                        {
                            if (OnTextChanged != null)
                                OnTextChanged(new SystemWindow(hwnd), (char)wParam);
                            break;
                        }
                    case NativeMethods.WindowsMessages.WM_KEYDOWN:
                        {
                            if (OnKeyPressed != null)
                                OnKeyPressed(new SystemWindow(hwnd), (Keys)wParam);

                            if ((Keys)wParam == Keys.Down)
                                return IntPtr.Zero; /// The edit box won't handle this key anyway so why should we bother it?
                            break;
                        }
                }
            } catch (Exception ex)
            {
                log.ErrorFormat("Exception in EditBoxHandler {0}", ex.ToString());
            }
            /// Send message for default window procedure of the attached edit box
            return NativeMethods.DefSubclassProc(hwnd, msg, wParam, lParam);
        }

    }
}
