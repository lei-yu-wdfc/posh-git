using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;

namespace Wonga.QA.Framework.Helpers
{
    public static class DateHelper
    {
        public static int GetNumberOfDaysUntilStartOfLoanForCa(DateTime? fromDate = null)
        {
            if (!fromDate.HasValue)
                fromDate = DateTime.Now;

            var nextWorkingDay = GetNextWorkingDay(fromDate.Value);

            return (int)(nextWorkingDay.Subtract(fromDate.Value)).TotalDays;
        }

        public static DateTime GetNextWorkingDay(DateTime date)
        {
            List<CalendarDateEntity> bankHolidays = GetBankHolidays(date, date.AddMonths(1));

            date = date.AddDays(Constants.Ca.DefaultDaysUntilStartOfLoan);

            while (IsNonWorkingDay(date, bankHolidays))
            {
                date = date.AddDays(1);
            }

            return date;
        }

        private static List<CalendarDateEntity> GetBankHolidays(DateTime startOfRange, DateTime endOfRange)
        {
            return
                Driver.Db.Payments.CalendarDates.Where(
                    itm => itm.IsBankHoliday && itm.Date >= startOfRange && itm.Date < endOfRange.AddDays(1)).
                    OrderBy(itm => itm.Date).ToList();
        }

        private static bool IsNonWorkingDay(DateTime date, List<CalendarDateEntity> bankHolidays)
        {
            return date.DayOfWeek == DayOfWeek.Sunday ||
                   date.DayOfWeek == DayOfWeek.Saturday ||
                   bankHolidays.Find(d => d.Date == date.Date) != null;
        }
    }
}