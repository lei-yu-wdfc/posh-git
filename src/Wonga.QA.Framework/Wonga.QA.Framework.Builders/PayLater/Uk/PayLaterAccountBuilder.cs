using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
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
            yield return SavePayLaterCustomerDetailsPayLaterUkCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.Forename = AccountData.Forename;
				r.Surname = AccountData.Surname;
				r.Email = AccountData.Email;
				r.DateOfBirth = AccountData.DateOfBirth;
				r.Title = TitleEnum.Mr;
                r.MobilePhone = AccountData.MobilePhoneNumber;
			});

            yield return RiskSavePayLaterCustomerDetailsPayLaterUkCommand.New(r =>
            {
                r.AccountId = AccountId;
                r.Forename = AccountData.Forename;
                r.Surname = AccountData.Surname;
                r.Email = AccountData.Email;
                r.DateOfBirth = AccountData.DateOfBirth;
                r.MobilePhone = AccountData.MobilePhoneNumber;
            });

            yield return SavePayLaterCustomerAddressPayLaterUkCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.HouseNumber = AccountData.HouseNumber;
				r.Postcode = AccountData.Postcode;
				r.Street = AccountData.Street;
				r.Flat = AccountData.Flat;
				r.Town = AccountData.Town;
			});

            yield return RiskSavePayLaterCustomerAddressPayLaterUkCommand.New(r =>
            {
                r.AccountId = AccountId;
                r.HouseNumber = AccountData.HouseNumber;
                r.Postcode = AccountData.Postcode;
                r.Street = AccountData.Street;
                r.Flat = AccountData.Flat;
                r.Town = AccountData.Town;
            });

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

            yield return RiskSavePayLaterEmploymentDetailsPayLaterUkCommand.New(r =>
            {
                r.AccountId = AccountId;
                r.EmploymentStatus = AccountData.EmploymentStatus;
                r.IncomeFrequency = AccountData.IncomeFrequency;
                r.NetIncome = AccountData.NetMonthlyIncome;
                r.NextPayDate = AccountData.NextPayDate;
            });
		}
	}
}