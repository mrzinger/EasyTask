using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Outlook = NetOffice.OutlookApi;

namespace EasyTaskAddin
{
    public class DueDateProcessor
    {
        public class HitPosition
        {            
            public int Start { get; set; }
            public int Length { get; set; }
        }

        public bool ReplaceDueDateString { get; set; } = true;

        public Outlook.TaskItem ParseAndSetDueDate(Outlook.TaskItem taskItem)
        {
            var parsedDate = DateTime.Today;
            HitPosition hitPosition = null;
            if (ParseKeywordsToDate(taskItem.Subject, out parsedDate, out hitPosition) == true)
            {
                if (parsedDate < DateTime.Today)  //we cannot assing duedate in the past
                    return taskItem;

                if (!taskItem.DueDate.Equals(parsedDate))
                    taskItem.DueDate = parsedDate;

                if (ReplaceDueDateString == true)
                    taskItem.Subject = taskItem.Subject.Remove(hitPosition.Start, hitPosition.Length);
            }

            return taskItem;
        }

        protected bool ParseKeywordsToDate(string input, out DateTime parsedDate, out HitPosition hitPosition)
        {
            // Get DateTime for the specified culture
            var results = DateTimeRecognizer.RecognizeDateTime(input, Culture.EnglishOthers);

            // Check there are valid results
            if (results.Count > 0 && results.First().TypeName.StartsWith("datetimeV2"))
            {
                
                var first = results.First();
                var resolutionValues = (IList<Dictionary<string, string>>)first.Resolution["values"];

                var subType = first.TypeName.Split('.').Last();

                hitPosition = new HitPosition
                {
                    Start = (first.Start > 0) ? first.Start - 1 : first.Start,
                    Length = first.Text.Length + 1
                };

                if (subType.Contains("date") && !subType.Contains("range"))
                {
                    // a date (or date & time) or multiple
                    parsedDate = resolutionValues.Select(v => DateTime.Parse(v["value"])).LastOrDefault();
                    return true;
                }
                else if (subType.Contains("date") && subType.Contains("range"))
                {
                    // We subtract one day in order to land on the last day of the month/week/year             
                    parsedDate = DateTime.Parse(resolutionValues.First()["end"]).AddDays(-1);

                    return true;
                }
            }

            hitPosition = new HitPosition
            {
                Start = 0,
                Length = 0
            };

            parsedDate = DateTime.Today;

            return false;
        }

    }
}
