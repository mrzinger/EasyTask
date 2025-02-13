using Accessibility;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace OutlookControlsExtensions
{
    public class WindowSubclassManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WindowSubclassManager));

        public class SubclassParameters 
        {
            public string wndAccName;
            public string wndClassName;
            public NativeMethods.WindowProc handlerProc;
            public int subclassId;
            public IntPtr hWnd;
        }

        private List<SubclassParameters> _subclassParams = new List<SubclassParameters>();
        /// <summary>
        /// Returns subclass parameters list associated with current instance of WindowSubclassManager.
        /// </summary>
        public List<SubclassParameters> Parameters
        {
            get
            {
                return _subclassParams;
            }
        }
        /// <summary>
        /// Callback function for searching windows with specified parameters (Clas Accessibility name)
        /// </summary>
        /// <param name="hwndChild"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private bool EnumChildProc(IntPtr hwndChild, ref IntPtr lParam)
        {
            StringBuilder buf = new StringBuilder(128);
            NativeMethods.GetClassName(hwndChild, buf, (IntPtr)128);

            SubclassParameters subClassParam = _subclassParams.Last();

            if (buf.ToString() == subClassParam.wndClassName)
            {
                UIntPtr OBJID_WINDOW = (UIntPtr)0x00000000;
                Guid IID_IAccessible = new Guid("618736E0-3C3D-11CF-810C-00AA00389B71");
                NativeMethods.IAccessible ptr;

                try
                {
                    int hr = NativeMethods.AccessibleObjectFromWindow(hwndChild, OBJID_WINDOW, IID_IAccessible.ToByteArray(), out ptr);
                    if (hr >= 0)
                    {
                        IAccessible accObj = (IAccessible)ptr;

                        log.DebugFormat("Window with class name {0} and Accesibility name {2} found. hWnd: 0x{1}", buf.ToString(), hwndChild.ToString("X"), accObj.get_accName());

                        if (accObj.get_accName() == subClassParam.wndAccName)
                        {
                            lParam = hwndChild;
                            subClassParam.hWnd = hwndChild;
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            return true;
        }
        /// <summary>
        /// Finds window by its ClassName and IAccessible.get_accName value and subclass it using specified wndProc
        /// </summary>
        /// <param name="parenthWnd">hWnd of parent window</param>
        /// <param name="wndClassName">Target window class name. If null the parameter is ommited.</param>
        /// <param name="wndAccName">Target window accessible name. If null the parameter is ommited.</param>
        /// <param name="wndProc">Window procedure to be called when the window is subclassed.</param>
        /// <param name="id">Unique identifier of subclass. If null the parameter is ommited.</param>
        /// <returns>Handler to found window</returns>
        public IntPtr FindAndSubclassWnd(IntPtr parenthWnd, string wndClassName, string wndAccName, NativeMethods.WindowProc wndProc, int id)
        {
            SubclassParameters scParams = new  SubclassParameters();
            scParams.wndClassName = wndClassName;
            scParams.wndAccName = wndAccName;
            scParams.handlerProc = wndProc;
            scParams.subclassId = id;

            // This is required to keep callback procedure alive. I.e. until _subclassParams is cleared subclass handler reference will not be desctoryed.
            _subclassParams.Add(scParams);

            scParams.hWnd = IntPtr.Zero;
            NativeMethods.EnumChildCallback cb = new NativeMethods.EnumChildCallback(EnumChildProc);
            NativeMethods.EnumChildWindows(parenthWnd, cb, ref scParams.hWnd);

            // We start monitoring ToDoView container when task edit box appears. By default there are no editboxes.
            SubclassWindow(scParams.hWnd, wndProc, scParams);

            return scParams.hWnd;
        }

        private void SubclassWindow(IntPtr hwnd, NativeMethods.WindowProc wndProc, SubclassParameters scParams)
        {
            IntPtr stub = IntPtr.Zero;
            if (NativeMethods.GetWindowSubclass(hwnd, wndProc, scParams.subclassId, ref stub) == false)
            {
                NativeMethods.SetWindowSubclass(hwnd, wndProc, scParams.subclassId, IntPtr.Zero);
            }
        }
        /// <summary>
        /// Removes subclass for window specified by SubclassParameters.
        /// </summary>
        /// <param name="sc">Subclass parameters/param>
        /// <returns></returns>
        public bool RemoveWindowSubclass (SubclassParameters sc, bool removeFromList)
        {
            if (removeFromList == true && _subclassParams.Remove(sc) == false)
                throw new Exception("Trying to remove subclass param that doesn't exist");

            return NativeMethods.RemoveWindowSubclass(sc.hWnd, sc.handlerProc, sc.subclassId);
        }
        /// <summary>
        /// Remove handlers that corresponds to criteria (hwnd and wndProc address)
        /// </summary>
        /// <param name="hwnd">Windows handler of the window that should be searched</param>
        /// <param name="wndProc">Address of handling procedure</param>
        /// <param name="subclassId">Unique subclass identifier. Can be zero.</param>
        /// <returns></returns>
        public int RemoveWindowSubclass (IntPtr hwnd, NativeMethods.WindowProc wndProc, int subclassId)
        {
            var removedCount = 0;
            foreach (var scParam in _subclassParams.Where(r => (r.handlerProc == wndProc && r.hWnd == hwnd && r.subclassId == subclassId)))
            {
                if (RemoveWindowSubclass(scParam, false) == true)
                    removedCount++;
            }

            _subclassParams.RemoveAll(r => (r.handlerProc == wndProc && r.hWnd == hwnd && r.subclassId == subclassId));
            return removedCount;
        }
        /// <summary>
        /// Remove all subclassed procedures
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public int RemoveWindowSubclass(IntPtr hwnd)
        {
            var removedCount = 0;
            foreach (var scParam in _subclassParams.Where(r => r.hWnd == hwnd ))
            {
                if (RemoveWindowSubclass(scParam, false) == true)
                    removedCount++;
            }
            _subclassParams.RemoveAll(r => r.hWnd == hwnd);
            return removedCount;
        }

        /// <summary>
        /// Removes all subclassed handlers and returns count of the removed handlers.
        /// </summary>
        /// <returns>Count of removed handlers</returns>
        public int Reset ()
        {
            foreach (var scParam in _subclassParams)
                RemoveWindowSubclass(scParam, false);
            
            int removedHandlers = _subclassParams.Count;
            _subclassParams.Clear();

            return removedHandlers;
        }
    }
}
