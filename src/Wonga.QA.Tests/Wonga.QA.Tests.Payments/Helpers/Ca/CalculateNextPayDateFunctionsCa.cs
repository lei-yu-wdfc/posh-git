using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Helpers.Ca
{
    public class CalculateNextPayDateFunctionsCa
    {
        private static DateTime GetFirstDayOfNextMonth(DateTime today)
        {
            return new DateTime(today.Year, today.Month, 1).AddMonths(1);
        }

        private static DateTime GetLastDayOfMonth(DateTime previousPayDate, DateTime today)
        {
            var stepBackPayday = GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousPayDate,
                                                                            GetFirstDayOfNextMonth(today).AddDays(-1));

            return stepBackPayday < today ? GetLastDayOfMonth(previousPayDate, today.AddDays(1)) : stepBackPayday;
        }

        private static DateTime GetLastFridayOfMonth(DateTime date)
        {
            DateTime firstDayOfNextMonth = GetFirstDayOfNextMonth(date);
            DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);
            while (lastDayOfMonth.DayOfWeek != DayOfWeek.Friday)
            {
                lastDayOfMonth = lastDayOfMonth.AddDays(-1);
            }
            return lastDayOfMonth;
        }

        private static DateTime GetLastFriday(DateTime today)
        {
            DateTime friday = GetLastFridayOfMonth(today);
            if (friday < today)
            {
                friday = GetLastFridayOfMonth(today.AddMonths(1));
            }
            return friday;
        }

        private static DateTime GetNextDayOfMonth(DateTime previousNextPayDay, DateTime date)
        {
            if (previousNextPayDay > date)
                return GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDay, previousNextPayDay);

            var nextPayDate = previousNextPayDay.AddMonths(1);

            var stepBackPayDate = GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDay, nextPayDate);

            return stepBackPayDate < date ? GetNextDayOfWeek(previousNextPayDay, date.AddDays(1)) : stepBackPayDate;
        }

        private static DateTime GetNextBiMonthly(DateTime previousNextPayDay, DateTime today)
        {
            if (previousNextPayDay > today)
                return GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDay, previousNextPayDay);
              
            var nextPayDay = new DateTime();

            if(today.Day < 15)
            {
                nextPayDay = new DateTime(today.Year, today.Month, 15);
            }
            else
            {
                nextPayDay = GetFirstDayOfNextMonth(today).AddDays(-1);
            }

            var stepBackPayDay = GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDay, nextPayDay);

            return stepBackPayDay < today ? GetNextBiMonthly(previousNextPayDay, today.AddDays(1)) : stepBackPayDay;
        }

        private static DateTime GetNextDayOfWeek(DateTime previousNextPayDate, DateTime today)
        {
            if (previousNextPayDate > today)
                return GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDate, previousNextPayDate);

            var nextPayDate = previousNextPayDate.AddDays(7);

            var stepBackPayDate = GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDate, nextPayDate);

            return stepBackPayDate < today ? GetNextDayOfWeek(previousNextPayDate, today.AddDays(1)) : stepBackPayDate;
        }

        private static DateTime GetNextBiWeeklyDate(DateTime previousNextPayDate, DateTime today)
        {
            if (previousNextPayDate > today)
                return GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDate, previousNextPayDate);

            var nextPayDate = previousNextPayDate.AddDays(14);

            var stepBackDate = GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDate, nextPayDate);

            return stepBackDate < today ? GetNextDayOfWeek(previousNextPayDate, today.AddDays(1)) : stepBackDate;

            //const int biWeeklyPeriod = 14;

            //DateTime nextPayDate = previousNextPayDate;

            //while (nextPayDate < DateTime.Today)
            //{
            //    nextPayDate = nextPayDate.AddDays(biWeeklyPeriod);
            //}

            //nextPayDate = GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(previousNextPayDate, nextPayDate);

            //return nextPayDate < today ? GetNextBiWeeklyDate(previousNextPayDate, today.AddDays(1)) : nextPayDate;
        }

        private static DateTime GetPreviousWorkingIfGivenPaydateIsNonWorkingDay(DateTime lastKnownPayDate, DateTime nextPayDate)
        {
            DateTime startOfRange = lastKnownPayDate;

            DateTime endOfRange = nextPayDate.AddMonths(1);

            List<CalendarDateEntity> bankHolidays = GetBankHolidays(startOfRange, endOfRange);

            const int goBackOneDay = -1;
            while (IsNonWorkingDay(nextPayDate, bankHolidays))
            {
                nextPayDate = nextPayDate.AddDays(goBackOneDay);
            }
            return nextPayDate;
        }

        private static List<CalendarDateEntity> GetBankHolidays(DateTime startOfRange, DateTime endOfRange)
        {
            return
                Drive.Db.Payments.CalendarDates.Where(
                    itm => itm.IsBankHoliday && itm.Date >= startOfRange && itm.Date < endOfRange.AddDays(1)).
                    OrderBy(itm => itm.Date).ToList();
        }

        private static bool IsNonWorkingDay(DateTime date, List<CalendarDateEntity> bankHolidays)
        {
            return date.DayOfWeek == DayOfWeek.Sunday ||
                   date.DayOfWeek == DayOfWeek.Saturday ||
                   bankHolidays.Find(d => d.Date == date.Date) != null;
        }

        public static DateTime CalculateNextPayDate(DateTime today, DateTime knownPayDate, PaymentFrequency incomeFrequency)
        {
            switch (incomeFrequency)
            {
                case PaymentFrequency.Monthly:
                    return GetNextDayOfMonth(knownPayDate, today);

                case PaymentFrequency.LastDayOfMonth:
                    return GetLastDayOfMonth(knownPayDate, today);

                case PaymentFrequency.LastFridayOfMonth:
                    return GetLastFriday(today);

                case PaymentFrequency.TwiceMonthly:
                    return GetNextBiMonthly(knownPayDate, today);

                case PaymentFrequency.Weekly:
                    return GetNextDayOfWeek(knownPayDate, today);

                case PaymentFrequency.BiWeekly:
                    return GetNextBiWeeklyDate(knownPayDate, today);
                case PaymentFrequency.TwiceMonthly15thAnd30th:
                    return GetNextDayOfMonth15thOr30th(today);
            }

            throw new Exception("Invalid incomeFrequency");
        }

        private static DateTime GetNextDayOfMonth15thOr30th(DateTime today)
        {
            int payMonth = today.Month;
            int payYear = today.Year;
            int payDay = 15;

            if (today.Day > 15 && today.Day <= 30)
            {
                payDay = 30;
                DateTime endOfTheMonth = GetFirstDayOfNextMonth(today).AddDays(-1);
                if (endOfTheMonth.Day < 30)
                {
                    payDay = endOfTheMonth.Day;
                }
            }

            if (today.Day == 31)
            {
                DateTime firstDayOfNextMonth = GetFirstDayOfNextMonth(today);
                payMonth = firstDayOfNextMonth.Month;
                payYear = firstDayOfNextMonth.Year;
            }

            return new DateTime(payYear, payMonth, payDay);
        }
    }
}
