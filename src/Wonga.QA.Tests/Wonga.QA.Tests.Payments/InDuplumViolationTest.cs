﻿using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using CreateTransactionCommand = Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages.CreateTransaction;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All)]
	public class InDuplumViolationTest
	{
		private dynamic _transactions = Drive.Data.Payments.Db.Transactions;
		private dynamic _applications = Drive.Data.Payments.Db.Applications;
		private Customer _customer;

		[Test, AUT(AUT.Za), JIRA("ZA-2201")]
		public void Close_InDuplumViolation_Application_Creates_CompensatingTransaction()
		{
			_customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
				Build();

			var loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());

			//fire refund transaction which cause InDuplum violation
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = 150,
				ApplicationId = app.Id,
				Currency = CurrencyCodeIso4217Enum.ZAR,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.UtcNow,
				Scope = PaymentTransactionScopeEnum.Debit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.Refund
			});

			//fire Direct Bank payment transaction to close off application so that compensating transaction 
			//will be created.
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = -200,
				ApplicationId = app.Id,
				Currency = CurrencyCodeIso4217Enum.ZAR,
				ExternalId = Guid.NewGuid(),
				ComponentTransactionId = Guid.Empty,
				PostedOn = DateTime.UtcNow,
				Scope = PaymentTransactionScopeEnum.Credit,
				Source = PaymentTransactionSourceEnum.System,
				Type = PaymentTransactionEnum.DirectBankPayment
			});

			//ClosedApplicationSaga timesout after 30sec before closing off application
			//Therefore delay is necessay
			Do.Until(() => app.IsClosed == false);

			//Can take up to 2 mins for transaction to be created
			var compensatingTransaction = Do.With.Timeout(2).Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId &&
																				_transactions.Type == PaymentTransactionEnum.WriteOff.ToString())
																				.FirstOrDefault());

			loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());

			Assert.IsNotNull(compensatingTransaction);

			//Calculations:
			//GetBal = 174.1 (100 + 57 + 17.1)
			//Refund = 150
			//LoanCap = 200
			//Therefore TrackedAmount 124.1 = (150 + 174.1) - 200
			Assert.AreEqual(-124.1m, compensatingTransaction.Amount);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2360"), DependsOn("Close_InDuplumViolation_Application_Creates_CompensatingTransaction")]
		public void Close_InDuplumViolation_Application_BalanceShouldBeZero()
		{
			Do.Until(() => decimal.Parse(GetLoanAgreements(_customer).Values["TodaysBalance"].SingleOrDefault()) == 0);
			Do.Until(() => decimal.Parse(GetLoanAgreements(_customer).Values["FinalBalance"].SingleOrDefault()) == 0);
		}

		#region Helpers

		private CsResponse GetLoanAgreements(Customer customer)
		{
			var query = new GetLoanAgreementsQuery() { AccountId = customer.Id, IsActive = null };
			return Drive.Cs.Queries.Post(query);
		}

		#endregion
	}
}