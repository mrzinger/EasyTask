using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTaskAddin
{
    public class SettingsHandler
    {
        DueDateProcessor m_dueDateProcessor = null;
        CategoryProcessor m_categoryProcessor = null;

        public SettingsHandler(DueDateProcessor dueDateProcessor, CategoryProcessor categoryProcessor)
        {
            m_dueDateProcessor = dueDateProcessor;
            m_categoryProcessor = categoryProcessor;
            UpdateSettings();
        }

        public void UpdateSettings()
        {
            var settings = Properties.Settings.Default;
            
            m_categoryProcessor.ReplaceCategoryString = settings.RemoveCategoryName;
            m_categoryProcessor.PatternType = (settings.UseKeywordCategoryPattern) ? CategoryProcessor.ParsePatternType.Keyword : 
                (settings.UseBracketCategoryPattern) ? CategoryProcessor.ParsePatternType.Brackets :
                (settings.UseArrowCategoryPattern) ? CategoryProcessor.ParsePatternType.Arrow :
                CategoryProcessor.ParsePatternType.CustomPattern;

            m_dueDateProcessor.ReplaceDueDateString = settings.RemoveDueDateString;
    
        }
    }
}
