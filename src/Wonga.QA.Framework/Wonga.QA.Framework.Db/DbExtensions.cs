using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Risk;

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

		public static bool IsWorkingDay(this DbDriver db, Date date)
		{
			if (db.IsHoliday(date)) return false;

			switch (Config.AUT)
			{
				case (AUT.Za):
					{
							if( date.DateTime.DayOfWeek == DayOfWeek.Sunday)
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
				serviceConfig = new ServiceConfigurationEntity {Key = key, Value = value};
				db.Ops.ServiceConfigurations.InsertOnSubmit(serviceConfig);
				db.Ops.SubmitChanges();
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

        public static void RemovePhoneNumberFromRiskDb(this DbDriver db, String mobilePhoneNumber)
        {
            //Clean the mobile number from DB 
            var riskDb = db.Risk;
            var entities = riskDb.RiskAccountMobilePhones.Where(p => p.MobilePhone == mobilePhoneNumber).ToList();
            if (entities.Count <= 0) return;
            riskDb.RiskAccountMobilePhones.DeleteAllOnSubmit(entities);
            riskDb.SubmitChanges();
        }

        public static void AddPhoneNumberToRiskDb(this DbDriver db, String mobilePhoneNumber)
        {
            var tempId = Guid.NewGuid();
            var riskDb = db.Risk;

            //Add the mobile number to Risk DB 
            var riskAccountEntity = new RiskAccountEntity()
            {
                AccountId = tempId,
                AccountRank = 1,
                HasAccount = true,
                CreditLimit = 400,
                ConfirmedFraud = false,
                DateOfBirth = Get.GetDoB(),
                DoNotRelend = false,
                Forename = Get.RandomString(8),
                IsDebtSale = false,
                IsDispute = false,
                IsHardship = false,
                Postcode = "KT2 5DL",
                MaidenName = Get.RandomString(8),
                Middlename = Get.RandomString(8),
                Surname = Get.RandomString(8)
            };
            var riskAccoutnMobilePhoneEntity = new RiskAccountMobilePhoneEntity()
            {
                AccountId = tempId,
                DateUpdated = new DateTime(2010, 10, 06).ToDate(),
                MobilePhone = mobilePhoneNumber,
            };

            riskDb.RiskAccounts.InsertOnSubmit(riskAccountEntity);
            riskDb.RiskAccountMobilePhones.InsertOnSubmit(riskAccoutnMobilePhoneEntity);
            riskDb.SubmitChanges();
        }

        public static void AddSurnameToBlacklist(this DbDriver db, String surname)
        {
            var blacklistEntity = new Blacklist.BlackListEntity()
                                      {
                                          LastName = surname,
                                          ExternalId = Guid.NewGuid(),
                                      };
            db.Blacklist.BlackLists.Insert(blacklistEntity);
            db.Blacklist.SubmitChanges();
        }
    }
}
