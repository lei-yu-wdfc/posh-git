using System;
using System.Collections.Generic;
using System.Diagnostics;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Account.Queries;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public abstract class PayLaterAccountBuilderBase
	{
		protected Guid AccountId { get; private set; }
		protected Guid MobilePhoneVerificationId { get; private set; }
		protected PayLaterAccountDataBase AccountData { get; private set; }


		protected PayLaterAccountBuilderBase(PayLaterAccountDataBase accountData) : this(Guid.NewGuid(), accountData){}

		protected PayLaterAccountBuilderBase(Guid accountId, PayLaterAccountDataBase accountData)
		{
			AccountId = accountId;
			AccountData = accountData;
			MobilePhoneVerificationId = Guid.NewGuid();
		}

		public PayLaterAccount Build()
		{
			CreateAccount();
			WaitUntilAccountIsPresentInServiceDatabases();
			CompleteMobilePhoneVerification();
			
			return new PayLaterAccount(AccountId);
		}

		protected void CreateAccount()
		{
			var commands = new List<ApiRequest>();
			commands.AddRange(GetGenericApiCommands());
            commands.AddRange(GetRegionSpecificApiCommands());
			Drive.Api.Commands.Post(commands);
		}

		protected IEnumerable<ApiRequest> GetGenericApiCommands()
		{
			yield return new CreateAccountCommand
			             	{
			             		AccountId = this.AccountId,
			             		Login = AccountData.Email,
			             		Password = AccountData.Password
			             	};

			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = MobilePhoneVerificationId);
		}

		abstract protected IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		private void WaitUntilAccountIsPresentInServiceDatabases()
		{            
            Do.Until(() => AccountQueries.PayLater.DataPresence.IsAccountPresentInServiceDatabases(AccountId));           
		}

		private void CompleteMobilePhoneVerification()
		{
			Drive.Api.Commands.Post(CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = MobilePhoneVerificationId));

			try
			{
				Do.Until(() => Drive.Data.Comms.Db.MobilePhoneVerification.FindByVerificationId(MobilePhoneVerificationId).MobileVerifiedOn != null);
			}

			catch (Exception)
			{
				throw new Exception(String.Format("Mobile phone not verified for AccountId: {0}", AccountId));
			}
		}

		#region "With" Methods

        public PayLaterAccountBuilderBase WithRiskTestMask(RiskMask mask)
        {
            AccountData.Surname += mask.ToString();
            return this;
        }

		public PayLaterAccountBuilderBase WithPassword(String value)
		{
			AccountData.Password = value;
			return this;
		}

		public PayLaterAccountBuilderBase WithDateOfBirth(Date value)
		{
			AccountData.DateOfBirth = value;
			return this;
		}

		public PayLaterAccountBuilderBase WithForename(String value)
		{
			AccountData.Forename = value;
			return this;
		}

		public PayLaterAccountBuilderBase WithSurname(String value)
		{
			AccountData.Surname = value;
			return this;
		}

		public PayLaterAccountBuilderBase WithEmail(String value)
		{
			AccountData.Email = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithMobilePhoneNumber(String value)
		{
			AccountData.MobilePhoneNumber = value;
			return this;
		}

		public PayLaterAccountBuilderBase WithFlat(String value)
		{
			AccountData.Flat = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithHouseNumber(String value)
		{
			AccountData.HouseNumber = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithStreet(String value)
		{
			AccountData.Street = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithTown(String value)
		{
			AccountData.Town = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithCounty(String value)
		{
			AccountData.County = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithPostcode(String value)
		{
			AccountData.Postcode = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithCountryCode(String value)
		{
			AccountData.CountryCode = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithEmploymentStatus(EmploymentStatusEnum value)
		{
			AccountData.EmploymentStatus = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithNextPayDate(Date value)
		{
			AccountData.NextPayDate = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithIncomeFrequency(IncomeFrequencyEnum value)
		{
			AccountData.IncomeFrequency = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithPaymentCardNumber(long value)
		{
			AccountData.PaymentCardNumber = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithPaymentCardSecurityCode(String value)
		{
			AccountData.PaymentCardSecurityCode = value;;
			return this;
		}

		public PayLaterAccountBuilderBase WithPaymentCardExpiryDate(Date value)
		{
			AccountData.PaymentCardExpiryDate = value;;
			return this;
		}

		#endregion
	}
}