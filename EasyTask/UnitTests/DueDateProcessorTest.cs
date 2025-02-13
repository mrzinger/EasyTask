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
            DateTime testDate = DateTime.Today;
            dueDateProcessor.ParseKeywordsToDateWrapper("Today", out testDate);
            Assert.AreEqual(DateTime.Today.ToLongDateString(), testDate.ToLongDateString());
        }

        private bool wAssert (DayOfWeek dow, DateTime date, int startDay) 
        {
            var todayDOW = DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek;
            var intDow = dow == DayOfWeek.Sunday ? 7 : (int)dow;
            Assert.AreEqual(dow, date.DayOfWeek);
            Assert.IsTrue(date.Subtract(DateTime.Today).Days == (startDay + intDow - todayDOW));
            return true;
        }

        [TestMethod()]
        public void ParseNextKeywordTest()
        {
            DueDateProcessorWrapper dueDateProcessor = new DueDateProcessorWrapper();

            DateTime testDate = DateTime.Today;
         
            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do next Monday", out testDate);
            wAssert(DayOfWeek.Monday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do Next Tuesday", out testDate);
            wAssert(DayOfWeek.Tuesday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do Next WednesdaY", out testDate);
            wAssert(DayOfWeek.Wednesday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do  next  thursday", out testDate);
            wAssert(DayOfWeek.Thursday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do  neXt frIdAy", out testDate);
            wAssert(DayOfWeek.Friday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do neXt Saturday", out testDate);
            wAssert(DayOfWeek.Saturday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do NeXt sunday", out testDate);
            wAssert(DayOfWeek.Sunday, testDate, 7);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do next week", out testDate);
            wAssert(DayOfWeek.Sunday, testDate, 7);

        }

        [TestMethod()]
        public void ParseThisKeywordTest()
        {
            DueDateProcessorWrapper dueDateProcessor = new DueDateProcessorWrapper();

            DateTime testDate = DateTime.Today;
            
            var dow = (int)DateTime.Today.DayOfWeek;

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this week", out testDate);
            wAssert(DayOfWeek.Sunday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this Monday", out testDate);
            wAssert(DayOfWeek.Monday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this Tuesday", out testDate);
            wAssert(DayOfWeek.Tuesday, testDate, 0);
            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do Tue", out testDate);
            wAssert(DayOfWeek.Tuesday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this WednesdaY", out testDate);
            wAssert(DayOfWeek.Wednesday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this thursday", out testDate);
            wAssert(DayOfWeek.Thursday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this frIdAy", out testDate);
            wAssert (DayOfWeek.Friday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do this Saturday", out testDate);
            wAssert(DayOfWeek.Saturday, testDate, 0);

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do ThiS  sunday", out testDate);
            wAssert(DayOfWeek.Sunday, testDate, 0);

        }

        [TestMethod()]
        public void TestParseDate()
        {
            DueDateProcessorWrapper dueDateProcessor = new DueDateProcessorWrapper();
            DateTime testDate = DateTime.Today;

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do 2020-12-31", out testDate);
            Assert.AreEqual("Thursday, December 31, 2020", testDate.ToLongDateString());

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do 12 of Feb", out testDate);
            Assert.AreEqual($"2/12/{DateTime.Today.Year}", testDate.ToShortDateString());

            dueDateProcessor.ParseKeywordsToDateWrapper("New Task to do first of December", out testDate);
            Assert.AreEqual($"12/1/{DateTime.Today.Year}", testDate.ToShortDateString());

        }

    }
}
