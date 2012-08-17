using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Account.Queries;
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
		protected Guid PrimaryPhoneVerificationId { get; private set; }
		protected ConsumerAccountDataBase AccountData { get; private set; }


		protected ConsumerAccountBuilderBase(ConsumerAccountDataBase accountData) : this(Guid.NewGuid(), accountData){}

		protected ConsumerAccountBuilderBase(Guid accountId, ConsumerAccountDataBase accountData)
		{
			AccountId = accountId;
			BankAccountId = Guid.NewGuid();
			PrimaryPhoneVerificationId = Guid.NewGuid();
			AccountData = accountData;
		}

		public ConsumerAccount Build()
		{
			CreateAccount();
			WaitUntilAccountIsPresentInServiceDatabases();

			return new ConsumerAccount(AccountId);
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
			                                      		r.Login = AccountData.Email;
			                                      		r.Password = AccountData.Password;
			                                      	});

			yield return SaveContactPreferencesCommand.New(r => r.AccountId = AccountId);
		}

		abstract protected IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		private void WaitUntilAccountIsPresentInServiceDatabases()
		{
			Do.Until(() => AccountQueries.Consumer.DataPresence.IsAccountPresentInServiceDatabases(AccountId));
		}

		#region "With" Methods - PersonalDetails

		public ConsumerAccountBuilderBase WithPassword(String password)
		{
			AccountData.Password = password;
			return this;
		}

		public ConsumerAccountBuilderBase WithGender(GenderEnum gender)
		{
			AccountData.Gender = gender;
			return this;
		}

		public ConsumerAccountBuilderBase WithNationalNumber(String nationalNumber)
		{
			AccountData.NationalNumber = nationalNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithDateOfBirth(Date date)
		{
			AccountData.DateOfBirth = date;
			return this;
		}

		public ConsumerAccountBuilderBase WithForename(String foreName)
		{
			AccountData.Forename = foreName;
			return this;
		}

		public ConsumerAccountBuilderBase WithMiddleName(String middleName)
		{
			AccountData.MiddleName = middleName;
			return this;
		}

		public ConsumerAccountBuilderBase WithSurname(String surname)
		{
			AccountData.Surname = surname;
			return this;
		}

		public ConsumerAccountBuilderBase WithMaidenName(String maidenName)
		{
			AccountData.MaidenName = maidenName;
			return this;
		}

		public ConsumerAccountBuilderBase WithNumberDependants(UInt16 numberOfDependants)
		{
			AccountData.NumberOfDependants = numberOfDependants;
			return this;
		}

		#endregion

		#region "With" Methods - Contact Details

		public ConsumerAccountBuilderBase WithEmailAddress(String email)
		{
			AccountData.Email = email;
			return this;
		}

		public ConsumerAccountBuilderBase WithPhoneNumber(String phoneNumber)
		{
			AccountData.HomePhoneNumber = phoneNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithMobileNumber(String mobileNumber)
		{
			AccountData.MobilePhoneNumber = mobileNumber;
			return this;
		}

		#endregion

		#region "With" Methods - Address Details
		
		public ConsumerAccountBuilderBase WithFlat(String flat)
		{
			AccountData.Flat = flat;
			return this;
		}

		public ConsumerAccountBuilderBase WithHouseNumber(String houseNumber)
		{
			AccountData.HouseNumber = houseNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithHouseName(String houseName)
		{
			AccountData.HouseName = houseName;
			return this;
		}

		public ConsumerAccountBuilderBase WithStreet(String street)
		{
			AccountData.Street = street;
			return this;
		}

		public ConsumerAccountBuilderBase WithDistrict(String district)
		{
			AccountData.District = district;
			return this;
		}

		public ConsumerAccountBuilderBase WithTown(String town)
		{
			AccountData.Town = town;
			return this;
		}

		public ConsumerAccountBuilderBase WithCounty(String county)
		{
			AccountData.County = county;
			return this;
		}

		public ConsumerAccountBuilderBase WithPostcode(String postcode)
		{
			AccountData.Postcode = postcode;
			return this;
		}

		public ConsumerAccountBuilderBase WithCountryCode(String countryCode)
		{
			AccountData.CountryCode = countryCode;
			return this;
		}

		#endregion

		#region "With" Methods - Employment Details

		public ConsumerAccountBuilderBase WithEmployer(String employerName)
		{
			AccountData.EmployerName = employerName;
			return this;
		}

		public ConsumerAccountBuilderBase WithEmployerStatus(String employerStatus)
		{
			AccountData.EmploymentStatus = employerStatus;
			return this;
		}

		public ConsumerAccountBuilderBase WithNetMonthlyIncome(Decimal netMonthlyIncome)
		{
			AccountData.NetMonthlyIncome = netMonthlyIncome;
			return this;
		}

		public ConsumerAccountBuilderBase WithNextPayDate(Date date)
		{
			AccountData.NextPayDate = date;
			return this;
		}

		public ConsumerAccountBuilderBase WithIncomeFrequency(IncomeFrequencyEnum incomeFrequency)
		{
			AccountData.IncomeFrequency = incomeFrequency;
			return this;
		}

		#endregion

		#region "With" Methods - Payment Details

		public ConsumerAccountBuilderBase WithBankAccountNumber( String bankAccountNumber)
		{
			AccountData.BankAccountNumber = bankAccountNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithBranchNumber(String branchNumber)
		{
			AccountData.BankBranchNumber = branchNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithBankCode(String bankCode)
		{
			AccountData.BankCode = bankCode;
			return this;
		}

		public ConsumerAccountBuilderBase WithPaymentCardNumber(Int64 cardNumber)
		{
			AccountData.PaymentCardNumber = cardNumber;
			return this;
		}

		public ConsumerAccountBuilderBase WithPaymentCardSecurityCode(String securityCode)
		{
			AccountData.PaymentCardSecurityCode = securityCode;
			return this;
		}

		public ConsumerAccountBuilderBase WithPaymentCardType(String cardType)
		{
			AccountData.PaymentCardType = cardType;
			return this;
		}
		#endregion
	}
}