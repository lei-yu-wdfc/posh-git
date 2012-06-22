using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
    [Parallelizable(TestScope.All)]
	public class FixedTermLoanRepayTest
	{
		[Test, AUT(AUT.Uk), JIRA("UK-921"), Description("")]
		[Row(55.99, false )]
		[Row(105.50, true)]
		public void LoanRepayTest(decimal repayAmount, bool fullRepay)
		{
			Customer cust = CustomerBuilder.New().Build();
			Do.Until(cust.GetBankAccount);
			Do.Until(cust.GetPaymentCard);
			Application app = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.WithLoanAmount(100)
				.WithLoanTerm(30)
				.Build();

			var ftApp = Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == app.Id);
			Assert.IsNotNull(ftApp);
		
			Assert.IsNull(GetTransactions(ftApp, repayAmount));

			decimal amount = GetAmount(cust.Id);

			Guid cardId = cust.GetPaymentCard();

			var cmd = new RepayLoanViaCardCommand
			{
				ApplicationId = app.Id,
				Amount = repayAmount,
				PaymentCardCv2 = "111",
				PaymentCardId = cardId,
				PaymentRequestId = Guid.NewGuid(),
			};

			var cmdAct = new Gallio.Common.Action(() => Drive.Api.Commands.Post(cmd));
			Assert.DoesNotThrow(cmdAct);

			Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() => GetTransactions(ftApp.ApplicationEntity.ExternalId, repayAmount));

			decimal newAmount = GetAmount(cust.Id);
			Assert.GreaterThan(amount, newAmount);

			if (fullRepay)
			{
				Guid appId = ftApp.ApplicationEntity.ExternalId;
				Do.With
					.Timeout(TimeSpan.FromSeconds(60))
					.Until(
						() => Drive.Db.Payments.FixedTermLoanApplications.Single(
							a =>
							a.ApplicationEntity.ExternalId == appId &&
							a.ApplicationEntity.ClosedOn != null));
			}
		}

		private TransactionEntity GetTransactions(Guid appId, decimal repayAmount)
		{
			var app = Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == appId);
			Assert.IsNotNull(app);
			return GetTransactions(app, repayAmount);
		}

		private TransactionEntity GetTransactions(FixedTermLoanApplicationEntity app, decimal repayAmount)
		{
			TransactionEntity paymentTransaction = app.ApplicationEntity.Transactions.FirstOrDefault(t => t.Amount == -repayAmount && t.Type == "CardPayment");
			return paymentTransaction;
		}

		private decimal GetAmount(Guid custId)
		{
			ApiResponse calc = Drive.Api.Queries.Post(
				new GetAccountSummaryQuery
					{
						AccountId = custId
					});

			string amount = calc.Values["CurrentLoanAmountDueToday"].Single();
			return decimal.Parse(amount);
		}
	}
}
