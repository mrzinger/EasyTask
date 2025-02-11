using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyTaskAddin
{
    public partial class SettingsForm : Form
    {
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            var settings = Properties.Settings.Default;
            rbtnStandardCategory.Checked = settings.UseKeywordCategoryPattern;
            rbtnUseBrackets.Checked = settings.UseBracketCategoryPattern;
            rbtnUseArrowCategoryPattern.Checked = settings.UseArrowCategoryPattern;
            rbtnCustomCategoryPattern.Checked = settings.UseCustomCategoryPattern;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.UseKeywordCategoryPattern = rbtnStandardCategory.Checked; 
            settings.UseBracketCategoryPattern = rbtnUseBrackets.Checked; 
            settings.UseArrowCategoryPattern = rbtnUseArrowCategoryPattern.Checked;
            settings.UseCustomCategoryPattern = rbtnCustomCategoryPattern.Checked; 
        }

        private void rbtnCustomCategoryPattern_CheckedChanged(object sender, EventArgs e)
        {
            tbCustomCategoryPattern.Enabled = rbtnCustomCategoryPattern.Checked;
        }
    }
}
