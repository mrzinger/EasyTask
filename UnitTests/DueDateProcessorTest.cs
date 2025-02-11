using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTaskAddin.Tests
{
    public class DueDateProcessorWrapper : DueDateProcessor
    {
        public bool ParseKeywordsToDateWrapper(string keyword, out DateTime parsedDate)
        {
            HitPosition ht = null;
            return base.ParseKeywordsToDate(keyword, out parsedDate, out ht);
        }

    }
    [TestClass()]
    public class DueDateProcessorTest
    {

        [TestMethod()]
        public void ParseTodayKeywordTest()
        {
            DueDateProcessorWrapper dueDateProcessor = new DueDateProcessorWrapper();
            DateTime testDate = DateTime.Now;
            dueDateProcessor.ParseKeywordsToDateWrapper("Today", out testDate);
            Assert.AreEqual(testDate.ToLongDateString(), DateTime.Now.ToLongDateString());
        }

        [TestMethod()]
        public void ParseNextKeywordTest()
        {
            DueDateProcessorWrapper dueDateProcessor = new DueDateProcessorWrapper();

            
            DateTime testDate = DateTime.Now;
        
           /* dueDateProcessor.ParseKeywordsToDateWrapper("next week", out testDate);
            Assert.AreEqual(DateTime.Now.AddDays(7).ToLongDateString(), testDate.ToLongDateString());

            dueDateProcessor.ParseKeywordsToDateWrapper("next Monday", out testDate);
            Assert.AreEqual(DayOfWeek.Monday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract (DateTime.Now).Days < 14);

            dueDateProcessor.ParseKeywordsToDateWrapper("Next Tuesday", out testDate);
            Assert.AreEqual(DayOfWeek.Tuesday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 14);

            dueDateProcessor.ParseKeywordsToDateWrapper("Next WednesdaY", out testDate);
            Assert.AreEqual(DayOfWeek.Wednesday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 14);

            dueDateProcessor.ParseKeywordsToDateWrapper("next  thursday", out testDate);
            Assert.AreEqual(DayOfWeek.Thursday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 14);

            dueDateProcessor.ParseKeywordsToDateWrapper("neXt frIdAy", out testDate);
            Assert.AreEqual(DayOfWeek.Friday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 14);

            dueDateProcessor.ParseKeywordsToDateWrapper("neXt Saturday", out testDate);
            Assert.AreEqual(DayOfWeek.Saturday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 14);

            dueDateProcessor.ParseKeywordsToDateWrapper("NeXt sunday", out testDate);
            Assert.AreEqual(DayOfWeek.Sunday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 14);*/

        }

        [TestMethod()]
        public void ParseThisKeywordTest()
        {
            DueDateProcessorWrapper dueDateProcessor = new DueDateProcessorWrapper();

            DateTime testDate = DateTime.Now;

            dueDateProcessor.ParseKeywordsToDateWrapper("this week", out testDate);
            Assert.AreEqual(DayOfWeek.Sunday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("this Monday", out testDate);
            Assert.AreEqual(DayOfWeek.Monday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("this Tuesday", out testDate);
            Assert.AreEqual(DayOfWeek.Tuesday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("this WednesdaY", out testDate);
            Assert.AreEqual(DayOfWeek.Wednesday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("this thursday", out testDate);
            Assert.AreEqual(DayOfWeek.Thursday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("this frIdAy", out testDate);
            Assert.AreEqual(DayOfWeek.Friday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("this Saturday", out testDate);
            Assert.AreEqual(DayOfWeek.Saturday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("ThiS  sunday", out testDate);
            Assert.AreEqual(DayOfWeek.Sunday, testDate.DayOfWeek);
            Assert.IsTrue(testDate.Subtract(DateTime.Now).Days < 7);

        }
      
    }
}
