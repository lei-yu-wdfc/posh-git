using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Tests.Core;
using SavePaymentCardBillingAddressCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.SavePaymentCardBillingAddressCommand;
using CurrencyCodeEnum = Wonga.QA.Framework.Api.Enums.CurrencyCodeEnum;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Payments.Command
{
	[TestFixture, Parallelizable((TestScope.All))]
	public class TakePaymentManualCsCommandTests
	{
		private static readonly dynamic Transactions = Drive.Data.Payments.Db.Transactions;
		private static readonly dynamic ApplicationStatusHistory = Drive.Data.BiCustomerManagement.Db.ApplicationStatusHistory;

		[Test, AUT(AUT.Wb), JIRA("SME-1161")]
		public void ShouldCreateTransaction_WhenCommandIsValid()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears() as BusinessApplication;

			Drive.Api.Commands.Post(new SavePaymentCardBillingAddressCommand
							{
								PaymentCardId = customer.GetPaymentCard(),
								HouseName = "Test",
								HouseNumber = "13",
								Street = "Sesame Street",
								Town = "Dublin",
								PostCode = "D1",
								CountryCode = "Irl"
							});

			Drive.Cs.Commands.Post(new TakePaymentManualCommand
							{
								Amount = 100.00m,
								ApplicationId = application.Id,
								Currency = CurrencyCodeEnum.GBP,
								PaymentCardId = customer.GetPaymentCard(),
								PaymentId = Guid.NewGuid(),
								SalesforceUser = "test.user@wonga.com"
							});

			var transactions = Drive.Data.Payments.Db.Transactions;

			Do.Until(() => transactions.FindAll(transactions.Amount == 100.00m
											&& transactions.ApplicationEntity.ExternalId == application.Id
											&& transactions.Scope == (int)PaymentTransactionScopeEnum.Credit
											&& transactions.Type == PaymentTransactionEnum.CardPayment.ToString()
											&& transactions.Reference == "Payment card repayment from CS"));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-1161")]
		public void ShouldReturnErrorAndNotInsertTransaction_WhenPaymentCardIsNotFound()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var exception = Assert.Throws<ValidatorException>(() => Drive.Cs.Commands.Post(new TakePaymentManualCommand
								{
									Amount = 100.00m,
									ApplicationId = application.Id,
									Currency = CurrencyCodeEnum.GBP,
									PaymentCardId = Guid.NewGuid(),
									PaymentId = Guid.NewGuid(),
									SalesforceUser = "test.user@wonga.com"
								}));
			Assert.Contains(exception.Errors, "Payments_PaymentCard_NotFound");


			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			Assert.AreEqual(0, Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: applicationId,
																			Type: PaymentTransactionEnum.CardPayment.ToString(),
																			Amount: 100.00m,
																			Scope: (int)PaymentTransactionScopeEnum.Credit,
																			Reference: "Payment card repayment from CS").Count());
		}

		[Test, AUT(AUT.Wb), JIRA("SME-1161")]
		public void ShouldReturnErrorAndNotInsertTransaction_WhenApplicationIsNotFound()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var exception = Assert.Throws<ValidatorException>(() => Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = 100.00m,
				ApplicationId = Guid.NewGuid(),
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = customer.GetPaymentCard(),
				PaymentId = Guid.NewGuid(),
				SalesforceUser = "test.user@wonga.com"
			}));
			Assert.Contains(exception.Errors, "Payments_Application_NotFound");

			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			Assert.AreEqual(0, Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: applicationId,
																			Type: PaymentTransactionEnum.CardPayment.ToString(),
																			Amount: 100.00m,
																			Scope: (int)PaymentTransactionScopeEnum.Credit,
																			Reference: "Payment card repayment from CS").Count());
		}

		[Test, AUT(AUT.Wb), JIRA("SME-1161")]
		public void ShouldReturnErrorAndNotInsertTransaction_WhenPaymentIdAlreadyExists()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var paymentId = Guid.NewGuid();

			Drive.Api.Commands.Post(new SavePaymentCardBillingAddressCommand
			{
				PaymentCardId = customer.GetPaymentCard(),
				HouseName = "Test",
				HouseNumber = "13",
				Street = "Sesame Street",
				Town = "Dublin",
				PostCode = "D1",
				CountryCode = "Irl"
			});

			Drive.Cs.Commands.Post(new TakePaymentManualCommand
									{
										Amount = 100.00m,
										ApplicationId = application.Id,
										Currency = CurrencyCodeEnum.GBP,
										PaymentCardId = customer.GetPaymentCard(),
										PaymentId = paymentId,
										SalesforceUser = "test.user@wonga.com"
									});

			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			Do.Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: applicationId,
																				Type: PaymentTransactionEnum.CardPayment.ToString(),
																				Amount: 100.00m,
																				Scope: (int)PaymentTransactionScopeEnum.Credit,
																				Reference: "Payment card repayment from CS").Count() == 1);

			var exception = Assert.Throws<ValidatorException>(() => Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = 100.00m,
				ApplicationId = application.Id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = customer.GetPaymentCard(),
				PaymentId = paymentId,
				SalesforceUser = "test.user@wonga.com"
			}));
			Assert.Contains(exception.Errors, "CsRepayWithPaymentCard_PaymentId_Known");

			Do.Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: applicationId,
																				Type: PaymentTransactionEnum.CardPayment.ToString(),
																				Amount: 100.00m,
																				Scope: (int)PaymentTransactionScopeEnum.Credit,
																				Reference: "Payment card repayment from CS").Count() == 1);
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-830"), Owner(Owner.ShaneMcHugh)]
		public void ShouldCreateTransaction_WhenCommandIsValid_Uk()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			Guid paymentId = Guid.NewGuid();
			Guid paymentCardId = customer.GetPaymentCard();
			TakePaymentManual(application, paymentId, paymentCardId);

			CheckDbForTransactionEntry(application);
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-830"), Owner(Owner.ShaneMcHugh)]
		public void ShouldReturnErrorAndNotInsertTransaction_WhenPaymentIdAlreadyExists_Uk()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			Guid paymentId = Guid.NewGuid();
			Guid paymentCardId = customer.GetPaymentCard();
			TakePaymentManual(application, paymentId, paymentCardId);

			CheckDbForTransactionEntry(application);

			var exception = Assert.Throws<ValidatorException>(() => TakePaymentManual(application, paymentId, paymentCardId));
			Assert.Contains(exception.Errors, "CsRepayWithPaymentCard_PaymentId_Known");

			CheckDbForTransactionEntry(application);
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-830"), Owner(Owner.ShaneMcHugh)]
		public void FullManualPayment_ShouldCreateTransactionAndStatusPaidInFull_WhenCommandIsValid_Uk()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			Guid paymentId = Guid.NewGuid();
			Guid paymentCardId = customer.GetPaymentCard();
			decimal amount = application.GetDueDateBalance();

			Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = amount,
				ApplicationId = application.Id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = paymentCardId,
				PaymentId = paymentId,
				SalesforceUser = "test.user@wonga.com"
			});

			Do.Until(() =>
					Transactions.FindAllBy(Reference: "Payment card repayment from CS, no card holder present",
										   ApplicationId: ApplicationOperations.GetAppInternalId(application),
										   Scope: (int)PaymentTransactionScopeEnum.Credit,
										   Type: PaymentTransactionEnum.CardPayment, Amount: amount));

			Do.Until(() =>
					 ApplicationStatusHistory.FindAllBy(
						 ApplicationId: ApplicationOperations.GetAppInternalId(application),
						 CurrentStatus: (double)Framework.ThirdParties.Salesforce.ApplicationStatus.PaidInFull,
						 AccountId: application.AccountId));
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-830"), Owner(Owner.ShaneMcHugh)]
		public void ShouldCreateTransaction_WhenLoanIsInArrears_Uk()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears();

			Guid paymentId = Guid.NewGuid();
			Guid paymentCardId = customer.GetPaymentCard();
			TakePaymentManual(application, paymentId, paymentCardId);

			CheckDbForTransactionEntry(application);
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-830"), Owner(Owner.ShaneMcHugh)]
		public void FullManualPayment_ShouldCreateTransactionAndStatusPaidInFull_WhenLoanIsInArrears_Uk()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build().PutIntoArrears(5);

			Guid paymentId = Guid.NewGuid();
			Guid paymentCardId = customer.GetPaymentCard();
			decimal amount = application.GetDueDateBalance();

			Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = amount,
				ApplicationId = application.Id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = paymentCardId,
				PaymentId = paymentId,
				SalesforceUser = "test.user@wonga.com"
			});

			Do.Until(() =>
					Transactions.FindAllBy(Reference: "Payment card repayment from CS, no card holder present",
										   ApplicationId: ApplicationOperations.GetAppInternalId(application),
										   Scope: (int)PaymentTransactionScopeEnum.Credit,
										   Type: PaymentTransactionEnum.CardPayment, Amount: amount));

			Do.Until(() =>
					 ApplicationStatusHistory.FindAllBy(
						 ApplicationId: ApplicationOperations.GetAppInternalId(application),
						 CurrentStatus: (double)Framework.ThirdParties.Salesforce.ApplicationStatus.PaidInFull,
						 AccountId: application.AccountId));
		}

		#region helpers#

		private void TakePaymentManual(dynamic application, Guid paymentId, Guid paymentCardId)
		{
			Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = 100.00m,
				ApplicationId = application.Id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = paymentCardId,
				PaymentId = paymentId,
				SalesforceUser = "test.user@wonga.com"
			});
		}

		private void CheckDbForTransactionEntry(dynamic application)
		{
			Do.Until(() =>
					Transactions.FindAllBy(Reference: "Payment card repayment from CS, no card holder present",
										   ApplicationId: ApplicationOperations.GetAppInternalId(application),
										   Scope: (int)PaymentTransactionScopeEnum.Credit,
										   Type: PaymentTransactionEnum.CardPayment, Amount: 100.00m));
		}

		#endregion helpers#

	}
}
