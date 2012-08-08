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
		public ConsumerAccountBuilder(ConsumerAccountDataBase consumerAccountData) : base(consumerAccountData)
		{
		}

		public ConsumerAccountBuilder(Guid accountId, ConsumerAccountDataBase consumerAccountData) : base(accountId, consumerAccountData)
		{
		}

		protected override IEnumerable<ApiRequest> GetRegionSpecificApiCommands()
		{
			yield return SaveSocialDetailsUkCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
																r.Dependants = ConsumerAccountData.NumberOfDependants;
			                                            	});

			yield return SavePasswordRecoveryDetailsUkCommand.New(r => r.AccountId = AccountId);

			yield return SaveCustomerDetailsUkCommand.New(r =>
			                                              	{
			                                              		r.AccountId = AccountId;
																r.Forename = ConsumerAccountData.Forename;
																r.MiddleName = ConsumerAccountData.MiddleName;
																r.Surname = ConsumerAccountData.Surname;
																r.Email = ConsumerAccountData.Email;
																r.DateOfBirth = ConsumerAccountData.DateOfBirth;
																r.HomePhone = ConsumerAccountData.HomePhoneNumber;
			                                              	});

			yield return RiskSaveCustomerDetailsUkCommand.New(r =>
			                                                  	{
			                                                  		r.AccountId = AccountId;
			                                                  		r.Forename = ConsumerAccountData.Forename;
			                                                  		r.MiddleName = ConsumerAccountData.MiddleName;
			                                                  		r.Surname = ConsumerAccountData.Surname;
																	r.Email = ConsumerAccountData.Email;
			                                                  		r.DateOfBirth = ConsumerAccountData.DateOfBirth;
			                                                  		r.MobilePhone = ConsumerAccountData.MobilePhoneNumber;
			                                                  		r.HomePhone = ConsumerAccountData.HomePhoneNumber;
			                                                  	});

			yield return SaveCustomerAddressUkCommand.New(r =>
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
			                                              	});

			yield return RiskSaveCustomerAddressUkCommand.New(r =>
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
			                                                  	});

			yield return AddBankAccountUkCommand.New(r =>
			                                         	{
			                                         		r.AccountId = AccountId;
			                                         		r.BankAccountId = BankAccountId;
															if (!string.IsNullOrEmpty(ConsumerAccountData.BankAccountNumber))
																r.AccountNumber = ConsumerAccountData.BankAccountNumber;
															if (!string.IsNullOrEmpty(ConsumerAccountData.BankCode))
																r.BankCode = ConsumerAccountData.BankCode;
			                                         	});

			yield return RiskAddBankAccountUkCommand.New(r =>
			                                             	{
			                                             		r.AccountId = AccountId;
			                                             		r.BankAccountId = BankAccountId;
			                                             		if (!string.IsNullOrEmpty(ConsumerAccountData.BankAccountNumber))
			                                             		{
			                                             			r.AccountNumber = ConsumerAccountData.BankAccountNumber;
			                                             		}
			                                             	});

			yield return AddPaymentCardCommand.New(r =>
			                                       	{
			                                       		r.AccountId = AccountId;
			                                       		r.Number = ConsumerAccountData.PaymentCardNumber;
			                                       		r.HolderName = String.Format("{0} {1}", ConsumerAccountData.Forename, ConsumerAccountData.Surname);
			                                       		r.IsPrimary = true;
			                                       		r.ExpiryDate = DateTime.Today.AddYears(2).ToPaymentCardDate();
			                                       		r.SecurityCode = ConsumerAccountData.PaymentCardSecurityCode;
			                                       		r.CardType = ConsumerAccountData.PaymentCardType;
			                                       	});

			yield return RiskAddPaymentCardCommand.New(r =>
			                                           	{
			                                           		r.AccountId = AccountId;
			                                           		r.Number = ConsumerAccountData.PaymentCardNumber;
			                                           		r.HolderName = String.Format("{0} {1}", ConsumerAccountData.Forename, ConsumerAccountData.Surname);
			                                           		r.ExpiryDate = DateTime.Today.AddYears(2).ToPaymentCardDate();
			                                           		r.SecurityCode = ConsumerAccountData.PaymentCardSecurityCode;
			                                           		r.CardType = ConsumerAccountData.PaymentCardType;
			                                           	});

			yield return SaveEmploymentDetailsUkCommand.New(r =>
			                                                	{
			                                                		r.AccountId = AccountId;
																	r.EmployerName = ConsumerAccountData.EmployerName;
																	r.Status = ConsumerAccountData.EmploymentStatus;
																	r.NetMonthlyIncome = ConsumerAccountData.NetMonthlyIncome;
			                                                	});

			yield return VerifyMobilePhoneUkCommand.New(r =>
			                                            	{
			                                            		r.AccountId = AccountId;
			                                            		r.VerificationId = MobilePhoneVerificationId;
			                                            		r.MobilePhone = ConsumerAccountData.MobilePhoneNumber;
			                                            	});

			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = MobilePhoneVerificationId);

			yield return RiskAddMobilePhoneUkCommand.New(r =>
			                                             	{
			                                             		r.AccountId = AccountId;
			                                             		r.MobilePhone = ConsumerAccountData.MobilePhoneNumber;
			                                             	});
		}
		
		protected override void CompletePhoneVerification(){}
	}
}