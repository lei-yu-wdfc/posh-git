using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater.Uk
{
	public class PayLaterAccountBuilder : PayLaterAccountBuilderBase
	{
		public PayLaterAccountBuilder(PayLaterAccountDataBase accountData) : base(accountData)
		{
		}

		public PayLaterAccountBuilder(Guid accountId, PayLaterAccountDataBase accountData) : base(accountId, accountData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			yield return SaveCustomerDetailsUkCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.Forename = AccountData.Forename;
				r.Surname = AccountData.Surname;
				r.Email = AccountData.Email;
				r.DateOfBirth = AccountData.DateOfBirth;
				r.Title = TitleEnum.Mr;
			});

			//yield return RiskPayLaterSaveCustomerDetailsUkCommand.New(r =>
			//{
			//    r.AccountId = AccountId;
			//    r.Forename = AccountData.Forename;
			//    r.Surname = AccountData.Surname;
			//    r.Email = AccountData.Email;
			//    r.DateOfBirth = AccountData.DateOfBirth;
			//    r.MobilePhone = AccountData.MobilePhoneNumber;
			//});

			yield return SaveCustomerAddressUkCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.HouseNumber = AccountData.HouseNumber;
				r.Postcode = AccountData.Postcode;
				r.Street = AccountData.Street;
				r.Flat = AccountData.Flat;
				r.Town = AccountData.Town;
			});

			//yield return RiskPayLaterSaveCustomerAddressUkCommand.New(r =>
			//{
			//    r.AccountId = AccountId;
			//    r.HouseNumber = AccountData.HouseNumber;
			//    r.Postcode = AccountData.Postcode;
			//    r.Street = AccountData.Street;
			//    r.Flat = AccountData.Flat;
			//    r.Town = AccountData.Town;
			//});

			yield return AddPaymentCardCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.Number = AccountData.PaymentCardNumber;
				r.HolderName = String.Format("{0} {1}", AccountData.Forename, AccountData.Surname);
				r.IsPrimary = true;
				r.ExpiryDate = AccountData.PaymentCardExpiryDate.ToPaymentCardDate();
				r.SecurityCode = AccountData.PaymentCardSecurityCode;
			});

			yield return RiskAddPaymentCardCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.Number = AccountData.PaymentCardNumber;
				r.HolderName = String.Format("{0} {1}", AccountData.Forename, AccountData.Surname);
				r.ExpiryDate = AccountData.PaymentCardExpiryDate.ToPaymentCardDate();
				r.SecurityCode = AccountData.PaymentCardSecurityCode;
			});
			
			//yield return SavePayLaterEmploymentDetailsUkCommand.New(r =>
			//{
			//    r.AccountId = AccountId;
			//    r.EmployerName = AccountData.EmployerName;
			//    r.Status = AccountData.EmploymentStatus;
			//    r.NetMonthlyIncome = AccountData.NetMonthlyIncome;
			//});
		}
	}
}