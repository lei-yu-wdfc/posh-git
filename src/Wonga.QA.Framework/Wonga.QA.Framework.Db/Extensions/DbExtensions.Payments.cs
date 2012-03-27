using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Db.Extensions
{
	public static partial class DbExtensions
	{
		public static bool IsHoliday(this DbDriver db, Date date)
		{
			return new DbDriver().Payments.CalendarDates.Any(a => a.IsBankHoliday && a.Date == date);
		}

		public static bool IsWorkingDay(this DbDriver db, Date date)
		{
			if (db.IsHoliday(date)) return false;

			switch (Config.AUT)
			{
				case (AUT.Za):
					{
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday)
							return false;
					}
					break;

				default:
					{
						if (date.DateTime.DayOfWeek == DayOfWeek.Saturday || date.DateTime.DayOfWeek == DayOfWeek.Sunday)
							return false;
					}
					break;
			}

			return true;
		}

		public static Date GetNextWorkingDay(this DbDriver db, Date date)
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(1);
						while (db.IsHoliday(date)) date.DateTime = date.DateTime.AddDays(1);
						return date;
					}

				default:
					{
						if (date.DateTime.DayOfWeek == DayOfWeek.Saturday) date.DateTime = date.DateTime.AddDays(2);
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(1);
						while (db.IsHoliday(date)) date.DateTime = date.DateTime.AddDays(1);
						return date;
					}
			}
		}

		public static Date GetPreviousWorkingDay(this DbDriver db, Date date)
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(-1);
						while (db.IsHoliday(date)) date.DateTime = date.DateTime.AddDays(-1);
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(-1);
						return date;
					}
				default:
					{
						if (date.DateTime.DayOfWeek == DayOfWeek.Saturday) date.DateTime = date.DateTime.AddDays(-1);
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(-2);
						while (db.IsHoliday(date)) date.DateTime = date.DateTime.AddDays(-1);
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(-2);
						if (date.DateTime.DayOfWeek == DayOfWeek.Saturday) date.DateTime = date.DateTime.AddDays(-1);
						return date;
					}
			}

		}

		public static int[] GetDefaultPayDaysOfMonth(this DbDriver db)
		{
			var value = db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.PayDayPerMonth").Value;
			return value.Split(',').Select(Int32.Parse).ToArray();
		}

	}
}
