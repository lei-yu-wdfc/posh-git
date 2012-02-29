using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;

namespace Wonga.QA.Framework.Db
{
    public static class DbExtensions
    {
        public static T Insert<T>(this Table<T> table, T entity) where T : DbEntity<T>
        {
            table.InsertOnSubmit(entity);
            return entity;
        }

        public static String GetName<T>(this Table<T> table) where T : DbEntity<T>
        {
            return table.Context.Mapping.GetTable(typeof(T)).TableName;
        }

		public static bool IsHoliday(this DbDriver db, Date date)
		{
			return new DbDriver().Payments.CalendarDates.Any(a => a.IsBankHoliday && a.Date == date);
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
						if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays( -2);
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

		public static void UpdateEmployerName(this DbDriver db, Guid accountId, string employerName)
		{
			db.Risk.EmploymentDetails.Single(a => a.AccountId == accountId).EmployerName = employerName;
			db.Risk.SubmitChanges();
		}

		public static ServiceConfigurationEntity GetServiceConfiguration(this DbDriver db, string key)
		{
			return db.Ops.ServiceConfigurations.SingleOrDefault(a => a.Key == key);
		}

		public static void SetServiceConfiguration(this DbDriver db, string key, string value)
		{
			var serviceConfig = db.GetServiceConfiguration(key);

			if( serviceConfig == null)
			{
				db.Ops.ServiceConfigurations.Insert(new ServiceConfigurationEntity {Key = key, Value = value});
				return;
			}

			serviceConfig.Value = value;
			serviceConfig.Submit();
		}

		public static void SetServiceConfigurations(this DbDriver db, Dictionary<string, string> keyValuePairs)
		{
			foreach (var keyValuePair in keyValuePairs)
			{
				db.SetServiceConfiguration(keyValuePair.Key, keyValuePair.Value);
			}
		}
    }
}
