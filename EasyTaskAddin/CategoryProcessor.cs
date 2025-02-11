using log4net;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Outlook = NetOffice.OutlookApi;
using System.Linq;

namespace EasyTaskAddin
{
    public class CategoryProcessor
    {
        private static readonly ILog log = LogManager.GetLogger("EasyTask");

        public enum ParsePatternType
        {
            Keyword,       //Assumes standard pattern with keyword "category:(CategoryName)"
            Brackets,       //Assumes pattern like "[CategoryName] Task description"
            Arrow,          //Assumes pattern like "CategoryName> Task description"
            CustomPattern   
        }

        private Dictionary<ParsePatternType, string> m_patternsDictionary = new Dictionary<ParsePatternType, string>() {
            { ParsePatternType.Keyword, @"category:\s?\((.+?)\)" },
            { ParsePatternType.Brackets, @"^\[(.+)\]" },
            { ParsePatternType.Arrow, @"^(.+)>" },
            { ParsePatternType.CustomPattern, "" }
        };

        public ParsePatternType PatternType { get; set; } = ParsePatternType.Keyword;

        public string CustomPattern
        {
            get { return m_patternsDictionary[ParsePatternType.CustomPattern]; }
            set { m_patternsDictionary[ParsePatternType.CustomPattern] = value; }
        }

        private bool m_replaceCategoryString = true;

        public bool ReplaceCategoryString
        {
            get { return m_replaceCategoryString; }
            set { m_replaceCategoryString = value; }
        }

        public Outlook.TaskItem ParseAndSetCategory(Outlook.TaskItem taskItem)
        {
            string strRegex = m_patternsDictionary[PatternType];
            Regex myRegex = new Regex(strRegex, RegexOptions.None);

            foreach (Match myMatch in myRegex.Matches(taskItem.Subject))
            {
                if (myMatch.Success)
                {
                    var categoryStr = myMatch.Groups[1].Value;
                    try
                    {
                        if (string.IsNullOrEmpty(taskItem.Categories))
                            taskItem.Categories = categoryStr;
                        else
                        {
                            var listSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator;
                            if (!taskItem.Categories.Split(listSeparator.ToCharArray()).Any(c => c == categoryStr))
                                taskItem.Categories += listSeparator + categoryStr;
                            
                        }
                        if (m_replaceCategoryString == true)
                        {
                            taskItem.Subject = taskItem.Subject.Replace(myMatch.Value, "").Trim();

                        }
                    }
                    catch (FormatException ex)
                    {
                        log.Error(ex.Data);
                    }

                }
            }
            return taskItem;
        }
    }
}
