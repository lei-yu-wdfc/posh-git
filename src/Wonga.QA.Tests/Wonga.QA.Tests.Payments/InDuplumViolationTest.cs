using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System.Threading;
using CreateTransactionCommand = Wonga.QA.Framework.Msmq.CreateTransactionCommand;

namespace Wonga.QA.Tests.Payments
{
	public class InDuplumViolationTest
	{
		private dynamic _transactions = Drive.Data.Payments.Db.Transactions;
		private dynamic _applications = Drive.Data.Payments.Db.Applications;

		[Parallelizable]
		[Test, AUT(AUT.Za), JIRA("ZA-2201")]
		public void Close_InDuplumViolation_Application_Creates_CompensatingTransaction()
		{
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
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
			Thread.Sleep(45000);

			var compensatingTransaction = Do.Until(() => _transactions.FindAll(	_transactions.ApplicationId == loanApp.ApplicationId &&
																				_transactions.Type == PaymentTransactionEnum.WriteOff.ToString())
																				.FirstOrDefault());

			loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());

			Assert.IsNotNull(compensatingTransaction);

			//Calculations:
			//GetBal = 174.1 (100 + 57 + 17.1)
			//Refund = 150
			//LoanCap = 200
			//Therefore TrackedAmount 124.1 = (150 + 174.1) - 200
			Assert.AreEqual(-124.1m ,compensatingTransaction.Amount);

		}

		[Parallelizable]
		[Test, AUT(AUT.Za), JIRA("ZA-2201")]
		public void Close_NonInDuplumViolation_Application_DoesNotCreate_CompensatingTransaction()
		{
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
				Build();

			var loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());


			//fire refund transaction which cause InDuplum violation
			Drive.Msmq.Payments.Send(new CreateTransactionCommand
			{
				Amount = 10,
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
				Amount = -184.1m, //Just enought to pay off Loan
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
			Thread.Sleep(45000);

			//Assert
			var compensatingTransaction = _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId && 
																_transactions.Type == PaymentTransactionEnum.WriteOff.ToString())
																.FirstOrDefault();

			loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());

			Assert.IsNull(compensatingTransaction);
		}

		[Parallelizable]
		[Test, AUT(AUT.Za), JIRA("ZA-2360")]
		public void Close_InDuplumViolation_Application_BalanceShouldBeZero()
		{
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
				Build();

			var loanApp = Do.Until(() => _applications.FindAllByExternalId(app.Id).Single());

			//fire refund transaction which cause InDuplum violation
			Drive.Msmq.Payments.Send(new Wonga.QA.Framework.Msmq.CreateTransactionCommand
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
			Drive.Msmq.Payments.Send(new Wonga.QA.Framework.Msmq.CreateTransactionCommand
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
			Thread.Sleep(45000);


			var query = new GetLoanAgreementsQuery() { AccountId = customer.Id, IsActive = null };
			CsResponse response = Drive.Cs.Queries.Post(query);
			Application[] applications = customer.GetApplications();

			//Assert.IsNotNull(response.Values["ClosedOn"].SingleOrDefault());
			Assert.AreEqual<decimal>(0, decimal.Parse(response.Values["TodaysBalance"].SingleOrDefault()));
			Assert.AreEqual<decimal>(0, decimal.Parse(response.Values["FinalBalance"].SingleOrDefault()));

		}
	}
}