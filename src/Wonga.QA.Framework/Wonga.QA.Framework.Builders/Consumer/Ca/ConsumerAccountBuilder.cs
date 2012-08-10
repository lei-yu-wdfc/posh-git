using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Ops.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca;

namespace Wonga.QA.Framework.Builders.Consumer.Ca
{
	public class ConsumerAccountBuilder : ConsumerAccountBuilderBase
	{
		public ConsumerAccountBuilder(ConsumerAccountDataBase accountData) : base(accountData)
		{
		}

		public ConsumerAccountBuilder(Guid accountId, ConsumerAccountDataBase accountData) : base(accountId, accountData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			yield return SaveSocialDetailsCaCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
			                                            		r.Dependants = AccountData.NumberOfDependants;
			                                            	});

			yield return SavePasswordRecoveryDetailsCaCommand.New(r => r.AccountId = AccountId);

			yield return SaveCustomerDetailsCaCommand.New(r =>
			                                              	{
			                                              		r.AccountId = AccountId;
			                                              		r.Forename = AccountData.Forename;
			                                              		r.MiddleName = AccountData.MiddleName;
			                                              		r.Surname = AccountData.Surname;
			                                              		r.Email = AccountData.Email;
			                                              		r.DateOfBirth = AccountData.DateOfBirth;
			                                              		r.NationalNumber = AccountData.NationalNumber;
			                                              		r.HomePhone = AccountData.HomePhoneNumber;
			                                              		r.Gender = AccountData.Gender;
			                                              	});

			yield return RiskSaveCustomerDetailsCaCommand.New(r =>
			                                                  	{
			                                                  		r.AccountId = AccountId;
			                                                  		r.Forename = AccountData.Forename;
			                                                  		r.MiddleName = AccountData.MiddleName;
			                                                  		r.Surname = AccountData.Surname;
			                                                  		r.Email = AccountData.Email;
			                                                  		r.DateOfBirth = AccountData.DateOfBirth;
			                                                  		r.HomePhone = AccountData.HomePhoneNumber;
			                                                  		r.Gender = AccountData.Gender;
			                                                  		r.MobilePhone = AccountData.MobilePhoneNumber;
			                                                  	});

			yield return SaveCustomerAddressCaCommand.New(r =>
			                                              	{
			                                              		r.AccountId = AccountId;
			                                              		r.HouseNumber = AccountData.HouseNumber;
			                                              		r.HouseName = AccountData.HouseName;
			                                              		r.Postcode = AccountData.Postcode;
			                                              		r.Street = AccountData.Street;
			                                              		r.Flat = AccountData.Flat;
			                                              		r.District = AccountData.District;
			                                              		r.Town = AccountData.Town;
			                                              		r.County = AccountData.County;
			                                              		r.Province = AccountData.Province;
			                                              	});

			yield return RiskSaveCustomerAddressCaCommand.New(r =>
			                                                  	{
			                                                  		r.AccountId = AccountId;
			                                                  		r.HouseNumber = AccountData.HouseNumber;
			                                                  		r.HouseName = AccountData.HouseName;
			                                                  		r.Postcode = AccountData.Postcode;
			                                                  		r.Street = AccountData.Street;
			                                                  		r.Flat = AccountData.Flat;
			                                                  		r.District = AccountData.District;
			                                                  		r.Town = AccountData.Town;
			                                                  		r.County = AccountData.County;
			                                                  		r.SubRegion = AccountData.Province;
			                                                  	});

			yield return AddBankAccountCaCommand.New(r =>
			                                         	{
			                                         		r.AccountId = AccountId;
			                                         		r.BankAccountId = BankAccountId;
			                                         		if (!string.IsNullOrEmpty(AccountData.BankAccountNumber))
			                                         			r.AccountNumber = AccountData.BankAccountNumber;
			                                         		r.BranchNumber = AccountData.BankBranchNumber;
			                                         		r.InstitutionNumber = AccountData.BankInstitutionNumber;
			                                         	});

			yield return RiskAddBankAccountCaCommand.New(r =>
			                                             	{
			                                             		r.AccountId = AccountId;
			                                             		r.BankAccountId = BankAccountId;
			                                             		if (!string.IsNullOrEmpty(AccountData.BankAccountNumber))
			                                             		{
			                                             			r.AccountNumber = AccountData.BankAccountNumber;
			                                             		}
			                                             	});

			yield return SaveEmploymentDetailsCaCommand.New(r =>
			                                                	{
			                                                		r.AccountId = AccountId;
			                                                		r.EmployerName = AccountData.EmployerName;
			                                                		r.NetMonthlyIncome = AccountData.NetMonthlyIncome;
			                                                		r.Status = AccountData.EmploymentStatus;
			                                                		if (!string.IsNullOrEmpty(AccountData.NextPayDate.ToString()))
			                                                			r.NextPayDate = AccountData.NextPayDate;
			                                                		if (!string.IsNullOrEmpty(AccountData.IncomeFrequency.ToString()))
			                                                			r.IncomeFrequency = AccountData.IncomeFrequency;
			                                                	});

			foreach (var api in GetPrimaryPhoneApiCommands())
				yield return api;
		}

		private IEnumerable<ApiRequest> GetPrimaryPhoneApiCommands()
		{
			if (AccountData.MobilePhoneNumber == null)
				return GetMobilePhoneApiCommands();

			return GetHomePhoneApiCommands();
		}

		private IEnumerable<ApiRequest> GetMobilePhoneApiCommands()
		{
			yield return VerifyMobilePhoneCaCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.VerificationId = PrimaryPhoneVerificationId;
				r.MobilePhone = AccountData.MobilePhoneNumber;
			});

			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = PrimaryPhoneVerificationId);

			yield return RiskAddMobilePhoneCaCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.MobilePhone = AccountData.MobilePhoneNumber;
			});
		}

		private IEnumerable<ApiRequest> GetHomePhoneApiCommands()
		{
			yield return VerifyHomePhoneCaCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.VerificationId = PrimaryPhoneVerificationId;
				r.HomePhone = AccountData.HomePhoneNumber;
			});

			yield return CompleteHomePhoneVerificationCaCommand.New(r => r.VerificationId = PrimaryPhoneVerificationId);

			yield return RiskAddHomePhoneCaCommand.New(r =>
			{
				r.AccountId = AccountId;
				r.HomePhone = AccountData.HomePhoneNumber;
			});
		}
	}
}
