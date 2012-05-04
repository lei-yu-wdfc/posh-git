using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
	public static class DateOperations
	{
		public static bool IsHoliday(this DateTime date)
		{
			return Drive.Data.Payments.Db.CalendarDates.FindBy(IsBankHoliday: true, Date: date.Date);
		}

		public static bool IsWorkingDay(this DateTime date)
		{
			switch (Config.AUT)
			{
				case (AUT.Za):
					if(date.DayOfWeek == DayOfWeek.Sunday)
					{
						return false;
					}
					break;
				default:
					if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
					{
						return false;
					}
					break;
			}

			return !date.IsHoliday();
		}

		public static DateTime GetNextWorkingDay(this DateTime date)
		{
			while(!date.IsWorkingDay())
			{
				date = date.AddDays(1.0);
			}

			return date.Date;
		}

		public static DateTime GetPreviousWorkingDay(this DateTime date)
		{
			while(!date.IsWorkingDay())
			{
				date = date.AddDays(-1.0);
			}

			return date.Date;
		}
	}
}
