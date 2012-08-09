using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Za;
using Wonga.QA.Framework.Api.Requests.Ops.Commands.Za;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Za;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Za;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer.Za
{
	public class ConsumerAccountBuilder : ConsumerAccountBuilderBase
	{
		public ConsumerAccountBuilder(ConsumerAccountDataBase consumerAccountData) : base(consumerAccountData)
		{
		}

		public ConsumerAccountBuilder(Guid accountId, ConsumerAccountDataBase consumerAccountData) : base(accountId, consumerAccountData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			yield return SaveSocialDetailsZaCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
			                                            		r.Dependants = AccountData.NumberOfDependants;
			                                            	});

			yield return SavePasswordRecoveryDetailsZaCommand.New(r => r.AccountId = AccountId);

			yield return SaveCustomerDetailsZaCommand.New(r =>
			                                              	{
			                                              		r.AccountId = AccountId;
			                                              		r.Forename = AccountData.Forename;
			                                              		r.MiddleName = AccountData.MiddleName;
			                                              		r.Surname = AccountData.Surname;
			                                              		r.Email = AccountData.Email;
			                                              		r.NationalNumber = AccountData.NationalNumber;
			                                              		r.DateOfBirth = AccountData.DateOfBirth;
			                                              		r.Gender = AccountData.Gender;
			                                              		r.MaidenName = AccountData.Gender == GenderEnum.Female
			                                              		               	? AccountData.MaidenName
			                                              		               	: null;
			                                              		r.HomePhone = AccountData.HomePhoneNumber;
			                                              	});

			yield return RiskSaveCustomerDetailsZaCommand.New(r =>
			                                                  	{
			                                                  		r.AccountId = AccountId;
			                                                  		r.Forename = AccountData.Forename;
			                                                  		r.MiddleName = AccountData.MiddleName;
			                                                  		r.Surname = AccountData.Surname;
			                                                  		r.Email = AccountData.Email;
			                                                  		r.DateOfBirth = AccountData.DateOfBirth;
			                                                  		r.Gender = AccountData.Gender;
			                                                  		r.MaidenName = AccountData.Gender == GenderEnum.Female
			                                                  		               	? AccountData.MaidenName
			                                                  		               	: null;
			                                                  		r.HomePhone = AccountData.HomePhoneNumber;
			                                                  		r.MobilePhone = AccountData.MobilePhoneNumber;
			                                                  	});

			yield return SaveCustomerAddressZaCommand.New(r =>
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
			                                              	});

			yield return RiskSaveCustomerAddressZaCommand.New(r =>
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
			                                                  	});

			yield return AddBankAccountZaCommand.New(r =>
			                                         	{
			                                         		r.AccountId = AccountId;
			                                         		r.BankAccountId = BankAccountId;
			                                         		if (!string.IsNullOrEmpty(AccountData.BankAccountNumber))
			                                         			r.AccountNumber = AccountData.BankAccountNumber;
			                                         	});

			yield return RiskAddBankAccountZaCommand.New(r =>
			                                             	{
			                                             		r.AccountId = AccountId;
			                                             		r.BankAccountId = BankAccountId;
			                                             		if (!string.IsNullOrEmpty(AccountData.BankAccountNumber))
			                                             			r.AccountNumber = AccountData.BankAccountNumber;
			                                             	});

			yield return SaveEmploymentDetailsZaCommand.New(r =>
			                                                	{
			                                                		r.AccountId = AccountId;
			                                                		r.EmployerName = AccountData.EmployerName;
			                                                		r.NextPayDate = AccountData.NextPayDate;
			                                                		r.Status = AccountData.EmploymentStatus;
			                                                	});

			yield return VerifyMobilePhoneZaCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
			                                            		r.VerificationId = PrimaryPhoneVerificationId;
			                                            		r.MobilePhone = AccountData.MobilePhoneNumber;
			                                            	});
		}

		protected override void CompletePhoneVerification(){}
	}
}
