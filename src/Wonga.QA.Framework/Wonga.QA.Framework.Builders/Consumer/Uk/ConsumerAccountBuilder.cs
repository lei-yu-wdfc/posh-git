using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Ops.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer.Uk
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
			yield return SaveSocialDetailsUkCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
																r.Dependants = AccountData.NumberOfDependants;
			                                            	});

			yield return SavePasswordRecoveryDetailsUkCommand.New(r => r.AccountId = AccountId);

			yield return SaveCustomerDetailsUkCommand.New(r =>
			                                              	{
			                                              		r.AccountId = AccountId;
																r.Forename = AccountData.Forename;
																r.MiddleName = AccountData.MiddleName;
																r.Surname = AccountData.Surname;
																r.Email = AccountData.Email;
																r.DateOfBirth = AccountData.DateOfBirth;
																r.HomePhone = AccountData.HomePhoneNumber;
			                                              	});

			yield return RiskSaveCustomerDetailsUkCommand.New(r =>
			                                                  	{
			                                                  		r.AccountId = AccountId;
			                                                  		r.Forename = AccountData.Forename;
			                                                  		r.MiddleName = AccountData.MiddleName;
			                                                  		r.Surname = AccountData.Surname;
																	r.Email = AccountData.Email;
			                                                  		r.DateOfBirth = AccountData.DateOfBirth;
			                                                  		r.MobilePhone = AccountData.MobilePhoneNumber;
			                                                  		r.HomePhone = AccountData.HomePhoneNumber;
			                                                  	});

			yield return SaveCustomerAddressUkCommand.New(r =>
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

			yield return RiskSaveCustomerAddressUkCommand.New(r =>
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

			yield return AddBankAccountUkCommand.New(r =>
			                                         	{
			                                         		r.AccountId = AccountId;
			                                         		r.BankAccountId = BankAccountId;
															if (!string.IsNullOrEmpty(AccountData.BankAccountNumber))
																r.AccountNumber = AccountData.BankAccountNumber;
															if (!string.IsNullOrEmpty(AccountData.BankCode))
																r.BankCode = AccountData.BankCode;
			                                         	});

			yield return RiskAddBankAccountUkCommand.New(r =>
			                                             	{
			                                             		r.AccountId = AccountId;
			                                             		r.BankAccountId = BankAccountId;
			                                             		if (!string.IsNullOrEmpty(AccountData.BankAccountNumber))
			                                             		{
			                                             			r.AccountNumber = AccountData.BankAccountNumber;
			                                             		}
			                                             	});

			yield return AddPaymentCardCommand.New(r =>
			                                       	{
			                                       		r.AccountId = AccountId;
			                                       		r.Number = AccountData.PaymentCardNumber;
			                                       		r.HolderName = String.Format("{0} {1}", AccountData.Forename, AccountData.Surname);
			                                       		r.IsPrimary = true;
			                                       		r.ExpiryDate = AccountData.PaymentCardExpiryDate;
			                                       		r.SecurityCode = AccountData.PaymentCardSecurityCode;
			                                       		r.CardType = AccountData.PaymentCardType;
			                                       	});

			yield return RiskAddPaymentCardCommand.New(r =>
			                                           	{
			                                           		r.AccountId = AccountId;
			                                           		r.Number = AccountData.PaymentCardNumber;
			                                           		r.HolderName = String.Format("{0} {1}", AccountData.Forename, AccountData.Surname);
															r.ExpiryDate = AccountData.PaymentCardExpiryDate;
			                                           		r.SecurityCode = AccountData.PaymentCardSecurityCode;
			                                           		r.CardType = AccountData.PaymentCardType;
			                                           	});

			yield return SaveEmploymentDetailsUkCommand.New(r =>
			                                                	{
			                                                		r.AccountId = AccountId;
																	r.EmployerName = AccountData.EmployerName;
																	r.Status = AccountData.EmploymentStatus;
																	r.NetMonthlyIncome = AccountData.NetMonthlyIncome;
			                                                	});

			yield return VerifyMobilePhoneUkCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
			                                            		r.VerificationId = PrimaryPhoneVerificationId;
			                                            		r.MobilePhone = AccountData.MobilePhoneNumber;
			                                            	});

			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = PrimaryPhoneVerificationId);

			yield return RiskAddMobilePhoneUkCommand.New(r =>
			                                             	{
			                                             		r.AccountId = AccountId;
			                                             		r.MobilePhone = AccountData.MobilePhoneNumber;
			                                             	});
		}
	}
}