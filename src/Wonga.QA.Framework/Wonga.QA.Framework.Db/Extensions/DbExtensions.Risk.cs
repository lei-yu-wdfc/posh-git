using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework.Db.Extensions
{
	public static partial class DbExtensions
	{
		public static void UpdateEmployerName(this DbDriver db, Guid accountId, string employerName)
		{
			db.Risk.EmploymentDetails.Single(a => a.AccountId == accountId).EmployerName = employerName;
			db.Risk.SubmitChanges();
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
	}
}
