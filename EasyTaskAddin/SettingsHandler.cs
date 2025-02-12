using OutlookControlsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTaskAddin
{
    public class SettingsHandler
    {
        DueDateProcessor _dueDateProcessor = null;
        CategoryProcessor _categoryProcessor = null;
        FilteredListExtension _filteredList = null;

        public SettingsHandler(DueDateProcessor dueDateProcessor, CategoryProcessor categoryProcessor, FilteredListExtension filteredListExtension)
        {
            _dueDateProcessor = dueDateProcessor;
            _categoryProcessor = categoryProcessor;
            _filteredList = filteredListExtension;
            UpdateSettings();
        }

        public void UpdateSettings()
        {
            var settings = Properties.Settings.Default;
            
            _categoryProcessor.ReplaceCategoryString = settings.RemoveCategoryName;
            _categoryProcessor.PatternType = (settings.UseKeywordCategoryPattern) ? CategoryProcessor.ParsePatternType.Keyword : 
                (settings.UseBracketCategoryPattern) ? CategoryProcessor.ParsePatternType.Brackets :
                (settings.UseArrowCategoryPattern) ? CategoryProcessor.ParsePatternType.Arrow :
                CategoryProcessor.ParsePatternType.CustomPattern;

            _dueDateProcessor.ReplaceDueDateString = settings.RemoveDueDateString;

            if (settings.UseBracketCategoryPattern)
                _filteredList.FilterTriggerChar = '[';
            else 
                _filteredList.FilterTriggerChar = null;
    
        }
    }
}
