using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Tests.Core;
using SavePaymentCardBillingAddressCommand = Wonga.QA.Framework.Api.SavePaymentCardBillingAddressCommand;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.Payments.Command
{
	[TestFixture, Parallelizable((TestScope.All))]
	public class TakePaymentManualCsCommandTests
	{
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
	}
}
