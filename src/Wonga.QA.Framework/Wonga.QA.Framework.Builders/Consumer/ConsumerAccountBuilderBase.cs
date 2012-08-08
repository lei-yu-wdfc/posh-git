using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
	public abstract class ConsumerAccountBuilderBase
	{
		protected Guid AccountId { get; private set; }
		protected Guid BankAccountId { get; private set; }
		protected Guid MobilePhoneVerificationId { get; private set; }
		protected ConsumerAccountDataBase ConsumerAccountData { get; private set; }


		protected ConsumerAccountBuilderBase(ConsumerAccountDataBase consumerAccountData) : this(Guid.NewGuid(), consumerAccountData){}

		protected ConsumerAccountBuilderBase(Guid accountId, ConsumerAccountDataBase consumerAccountData)
		{
			AccountId = accountId;
			BankAccountId = Guid.NewGuid();
			MobilePhoneVerificationId = Guid.NewGuid();
			ConsumerAccountData = consumerAccountData;
		}

		public Customer Build()
		{
			CreateAccount();
			WaitUntilAccountIsPresentInServiceDatabases();
			CompletePhoneVerification();

			return new Customer(AccountId);
		}

		protected void CreateAccount()
		{
			var commands = new List<ApiRequest>();

			commands.AddRange(GetGenericApiCommands());
			commands.AddRange(GetRegionSpecificApiCommands());

			Drive.Api.Commands.Post(commands);
		}

		private IEnumerable<ApiRequest> GetGenericApiCommands()
		{
			yield return CreateAccountCommand.New(r =>
			                                      	{
			                                      		r.AccountId = AccountId;
			                                      		r.Login = ConsumerAccountData.Email;
			                                      		r.Password = ConsumerAccountData.Password;
			                                      	});

			yield return SaveContactPreferencesCommand.New(r => r.AccountId = AccountId);
		}

		abstract protected IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		abstract protected void CompletePhoneVerification();

		private void WaitUntilAccountIsPresentInServiceDatabases()
		{
			Do.Until(() => Drive.Data.Ops.Db.Accounts.FindByExternalId(AccountId));
			Do.Until(() => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(AccountId));
			Do.Until(() => Drive.Data.Payments.Db.AccountPreferences.FindByAccountId(AccountId));
			Do.Until(() => Drive.Data.Risk.Db.RiskAccounts.FindByAccountId(AccountId));
		}

		#region "With" Methods - PersonalDetails

		public ConsumerAccountBuilderBase WithPassword(String password)
		{
			ConsumerAccountData.Password = password;
			return this;
		}

		public ConsumerAccountBuilderBase WithGender(GenderEnum gender)
		{
			ConsumerAccountData.Gender = gender;
			return this;
		}

		public ConsumerAccountBuilderBase WithNationalNumber(String nationalNumber)
		{
			ConsumerAccountData.NationalNumber = nationalNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithDateOfBirth(Date date)
		{
			ConsumerAccountData.DateOfBirth = date;
			return this;
		}

		public ConsumerAccountBuilderBase WithForename(String foreName)
		{
			ConsumerAccountData.Forename = foreName;
			return this;
		}

		public ConsumerAccountBuilderBase WithMiddleName(String middleName)
		{
			ConsumerAccountData.MiddleName = middleName;
			return this;
		}

		public ConsumerAccountBuilderBase WithSurname(String surname)
		{
			ConsumerAccountData.Surname = surname;
			return this;
		}

		public ConsumerAccountBuilderBase WithMaidenName(String maidenName)
		{
			ConsumerAccountData.MaidenName = maidenName;
			return this;
		}

		public ConsumerAccountBuilderBase WithNumberDependants(UInt16 numberOfDependants)
		{
			ConsumerAccountData.NumberOfDependants = numberOfDependants;
			return this;
		}

		#endregion

		#region "With" Methods - Contact Details

		public ConsumerAccountBuilderBase WithEmailAddress(String email)
		{
			ConsumerAccountData.Email = email;
			return this;
		}

		public ConsumerAccountBuilderBase WithPhoneNumber(String phoneNumber)
		{
			ConsumerAccountData.HomePhoneNumber = phoneNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithMobileNumber(String mobileNumber)
		{
			ConsumerAccountData.MobilePhoneNumber = mobileNumber;
			return this;
		}

		#endregion

		#region "With" Methods - Address Details
		
		public ConsumerAccountBuilderBase WithFlat(String flat)
		{
			ConsumerAccountData.Flat = flat;
			return this;
		}

		public ConsumerAccountBuilderBase WithHouseNumber(String houseNumber)
		{
			ConsumerAccountData.HouseNumber = houseNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithHouseName(String houseName)
		{
			ConsumerAccountData.HouseName = houseName;
			return this;
		}

		public ConsumerAccountBuilderBase WithStreet(String street)
		{
			ConsumerAccountData.Street = street;
			return this;
		}

		public ConsumerAccountBuilderBase WithDistrict(String district)
		{
			ConsumerAccountData.District = district;
			return this;
		}

		public ConsumerAccountBuilderBase WithTown(String town)
		{
			ConsumerAccountData.Town = town;
			return this;
		}

		public ConsumerAccountBuilderBase WithCounty(String county)
		{
			ConsumerAccountData.County = county;
			return this;
		}

		public ConsumerAccountBuilderBase WithPostcode(String postcode)
		{
			ConsumerAccountData.Postcode = postcode;
			return this;
		}

		public ConsumerAccountBuilderBase WithCountryCode(String countryCode)
		{
			ConsumerAccountData.CountryCode = countryCode;
			return this;
		}

		#endregion

		#region "With" Methods - Employment Details

		public ConsumerAccountBuilderBase WithEmployer(String employerName)
		{
			ConsumerAccountData.EmployerName = employerName;
			return this;
		}

		public ConsumerAccountBuilderBase WithEmployerStatus(String employerStatus)
		{
			ConsumerAccountData.EmploymentStatus = employerStatus;
			return this;
		}

		public ConsumerAccountBuilderBase WithNetMonthlyIncome(Decimal netMonthlyIncome)
		{
			ConsumerAccountData.NetMonthlyIncome = netMonthlyIncome;
			return this;
		}

		public ConsumerAccountBuilderBase WithNextPayDate(Date date)
		{
			ConsumerAccountData.NextPayDate = date;
			return this;
		}

		public ConsumerAccountBuilderBase WithIncomeFrequency(IncomeFrequencyEnum incomeFrequency)
		{
			ConsumerAccountData.IncomeFrequency = incomeFrequency;
			return this;
		}

		#endregion

		#region "With" Methods - Payment Details

		public ConsumerAccountBuilderBase WithBankAccountNumber( String bankAccountNumber)
		{
			ConsumerAccountData.BankAccountNumber = bankAccountNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithBranchNumber(String branchNumber)
		{
			ConsumerAccountData.BranchNumber = branchNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithBankCode(String bankCode)
		{
			ConsumerAccountData.BankCode = bankCode;
			return this;
		}

		public ConsumerAccountBuilderBase WithPaymentCardNumber(Int64 cardNumber)
		{
			ConsumerAccountData.PaymentCardNumber = cardNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithPaymentCardSecurityCode(String securityCode)
		{
			ConsumerAccountData.PaymentCardSecurityCode = securityCode;
			return this;
		}

		public ConsumerAccountBuilderBase WithPaymentCardType(String cardType)
		{
			ConsumerAccountData.PaymentCardType = cardType;
			return this;
		}
		#endregion
	}
}