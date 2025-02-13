using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = NetOffice.OutlookApi;

namespace EasyTaskAddin
{

    //This database is used to save the tasks that has been processed during tha add-in work. 
    //Required to avoid multiple handling of the same task due to circular TaskItem.Save() method call
    class ProcessedTasksDatabase
    {
        Dictionary<string, string> _taskDb = new Dictionary<string, string>();

        //Initialize database with existing items
        public ProcessedTasksDatabase(Outlook.Items items)
        {
            foreach (Outlook.TaskItem item in items)
            {
                _taskDb[item.EntryID] = item.Subject;
            }

        }

        //Returns true if there is already task in the database and its subject hasn't changed.
        //Otherwise adds a new record to db or update existing item subject.
        public bool CheckAndUpdate(Outlook.TaskItem item)
        {
            var taskId = item.EntryID;
            var taskSubject = item.Subject;

            if (_taskDb.Any(t => t.Key == taskId && t.Value == taskSubject) == true)
                return true;

            _taskDb[taskId] = taskSubject;
            return false;
        }
    }
}
