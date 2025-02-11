using NetOffice.OfficeApi;
using System;
using System.Drawing;
using Outlook = NetOffice.OutlookApi;

namespace EasyTaskAddin
{
    public class BaseRibbonHelper: Outlook.Tools.COMAddin
    {
        #region Helpers
        public Bitmap LoadImage(IRibbonControl control)
        {
            var imageName = GetParamFromTag(control.Tag, "image");

            if (imageName == String.Empty)
                return null;

            return (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject(imageName, Properties.Resources.Culture);
        }

        public string GetLabel(IRibbonControl control)
        {
            var labelName = GetParamFromTag(control.Tag, "label");

            if (labelName == String.Empty)
                return null;

            return Properties.Resources.ResourceManager.GetString(labelName, Properties.Resources.Culture);
        }

        public string GetDescription(IRibbonControl control)
        {
            var labelName = GetParamFromTag(control.Tag, "description");

            if (labelName == String.Empty)
                return null;

            return Properties.Resources.ResourceManager.GetString(labelName, Properties.Resources.Culture);
        }

        private static string GetParamFromTag(string tag, string paramName)
        {
            tag = tag.Replace(" ", "");
            var valueStartIndex = tag.LastIndexOf(paramName + "=");
            if (valueStartIndex == -1)
                return String.Empty;
            valueStartIndex += paramName.Length + 1;

            var valueEndIndex = tag.IndexOf(';', valueStartIndex);
            if (valueEndIndex == -1)
                valueEndIndex = tag.Length;


            return tag.Substring(valueStartIndex, valueEndIndex - valueStartIndex);
        }

        #endregion
    }
}
