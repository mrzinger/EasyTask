using ManagedWinapi.Windows;
using System.Windows.Forms;

namespace OutlookControlsExtensions
{
    public struct CurrentStringParams
    {
        public int StartPos;
        public int EndPos;
        public int CarretPos;
        public string CurText;
    }

    public interface ITaskEditBoxExtension
    {
        Control GetControl();
        bool OnTextChanged(SystemWindow triggerControl, string editBoxString, CurrentStringParams csParams, int cursorPosition);
        void OnEditBoxCreated(SystemWindow triggerControl);
        void OnEditBoxDestroyed(SystemWindow triggerControl);
        bool OnKeyPressed(SystemWindow triggerControl, Keys key);
    }
}
