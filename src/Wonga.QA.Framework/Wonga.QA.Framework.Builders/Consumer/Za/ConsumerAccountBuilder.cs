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
			return new ApiRequest[]
			       	{
			       		SaveSocialDetailsZaCommand.New(r =>
			       		                               	{
			       		                               		r.AccountId = AccountId;
			       		                               		r.Dependants = ConsumerAccountData.NumberOfDependants;
			       		                               	}),
			       		SavePasswordRecoveryDetailsZaCommand.New(r => r.AccountId = AccountId),
			       		SaveCustomerDetailsZaCommand.New(r =>
			       		                                 	{
			       		                                 		r.AccountId = AccountId;
			       		                                 		r.Forename = ConsumerAccountData.Forename;
			       		                                 		r.MiddleName = ConsumerAccountData.MiddleName;
			       		                                 		r.Surname = ConsumerAccountData.Surname;
			       		                                 		r.Email = ConsumerAccountData.Email;
			       		                                 		r.NationalNumber = ConsumerAccountData.NationalNumber;
			       		                                 		r.DateOfBirth = ConsumerAccountData.DateOfBirth;
			       		                                 		r.Gender = ConsumerAccountData.Gender;
			       		                                 		r.MaidenName = ConsumerAccountData.Gender == GenderEnum.Female ? ConsumerAccountData.MaidenName : null;
			       		                                 		r.HomePhone = ConsumerAccountData.HomePhoneNumber;
			       		                                 	}),
			       		RiskSaveCustomerDetailsZaCommand.New(r =>
			       		                                     	{
			       		                                     		r.AccountId = AccountId;
			       		                                     		r.Forename = ConsumerAccountData.Forename;
			       		                                     		r.MiddleName = ConsumerAccountData.MiddleName;
			       		                                     		r.Surname = ConsumerAccountData.Surname;
			       		                                     		r.Email = ConsumerAccountData.Email;
			       		                                     		r.DateOfBirth = ConsumerAccountData.DateOfBirth;
			       		                                     		r.Gender = ConsumerAccountData.Gender;
			       		                                     		r.MaidenName = ConsumerAccountData.Gender == GenderEnum.Female ? ConsumerAccountData.MaidenName : null;
			       		                                     		r.HomePhone = ConsumerAccountData.HomePhoneNumber;
			       		                                     		r.MobilePhone = ConsumerAccountData.MobilePhoneNumber;
			       		                                     	}),
			       		SaveCustomerAddressZaCommand.New(r =>
			       		                                 	{
			       		                                 		r.AccountId = AccountId;
			       		                                 		r.HouseNumber = ConsumerAccountData.HouseNumber;
			       		                                 		r.HouseName = ConsumerAccountData.HouseName;
			       		                                 		r.Postcode = ConsumerAccountData.Postcode;
			       		                                 		r.Street = ConsumerAccountData.Street;
			       		                                 		r.Flat = ConsumerAccountData.Flat;
			       		                                 		r.District = ConsumerAccountData.District;
			       		                                 		r.Town = ConsumerAccountData.Town;
			       		                                 		r.County = ConsumerAccountData.County;
			       		                                 	}),
			       		RiskSaveCustomerAddressZaCommand.New(r =>
			       		                                     	{
			       		                                     		r.AccountId = AccountId;
			       		                                     		r.HouseNumber = ConsumerAccountData.HouseNumber;
			       		                                     		r.HouseName = ConsumerAccountData.HouseName;
			       		                                     		r.Postcode = ConsumerAccountData.Postcode;
			       		                                     		r.Street = ConsumerAccountData.Street;
			       		                                     		r.Flat = ConsumerAccountData.Flat;
			       		                                     		r.District = ConsumerAccountData.District;
			       		                                     		r.Town = ConsumerAccountData.Town;
			       		                                     		r.County = ConsumerAccountData.County;
			       		                                     	}),
			       		AddBankAccountZaCommand.New(r =>
			       		                            	{
															r.AccountId = AccountId;
			       		                            		r.BankAccountId = BankAccountId;
			       		                            		if (!string.IsNullOrEmpty(ConsumerAccountData.BankAccountNumber))
			       		                            			r.AccountNumber = ConsumerAccountData.BankAccountNumber;
			       		                            	}),
			       		RiskAddBankAccountZaCommand.New(r =>
			       		                                	{
																r.AccountId = AccountId;
			       		                                		r.BankAccountId = BankAccountId;
			       		                                		if (!string.IsNullOrEmpty(ConsumerAccountData.BankAccountNumber))
			       		                                			r.AccountNumber = ConsumerAccountData.BankAccountNumber;
			       		                                	}),
			       		SaveEmploymentDetailsZaCommand.New(r =>
			       		                                   	{
																r.AccountId = AccountId;
			       		                                   		r.EmployerName = ConsumerAccountData.EmployerName;
			       		                                   		r.NextPayDate = ConsumerAccountData.NextPayDate;
			       		                                   		r.Status = ConsumerAccountData.EmploymentStatus;
			       		                                   	}),
			       		VerifyMobilePhoneZaCommand.New(r =>
			       		                               	{
															r.AccountId = AccountId;
			       		                               		r.VerificationId = MobilePhoneVerificationId;
			       		                               		r.MobilePhone = ConsumerAccountData.MobilePhoneNumber;
			       		                               	}),
			       	};
		}

		protected override void CompletePhoneVerification(){}
	}
}
