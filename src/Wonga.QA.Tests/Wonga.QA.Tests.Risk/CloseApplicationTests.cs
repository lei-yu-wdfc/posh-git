using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
	[TestFixture]
	public class CloseApplicationTests
	{
		[Test, AUT(AUT.Uk), JIRA("WIN-1125")]
		public void ShouldCloseApplicationButNotAddTransactionDetailsToRiskApplication_WhenTransactionDetailsDontExist()
		{
			var closedOn = DateTime.Today.AddDays(-3);

			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			Drive.Msmq.Risk.Send(new IApplicationClosed
			                         	{
			                         		ApplicationId = application.Id,
											AccountId = application.AccountId,
											ClosedOn = closedOn
			                         	});

			Do.Until<bool>(() => Drive.Data.Risk.Db.RiskApplications.FindAllBy(ApplicationId: application.Id,
																				PaymentTransactionCount: null,
																				ManualPaymentTransactionCount: null,
																				MainCardRepayRate: null,
																				ClosedOn: closedOn).Count() == 1);
		}

		[Test, AUT(AUT.Uk), JIRA("WIN-1125"), Owner(Owner.GuerganaYordanova)]
		public void ShouldCloseApplicationAndAddTransactionDetailsToRiskApplication_WhenCustomerHasRepaidLoan()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			application.RepayOnDueDate();
		
			Do.Until<bool>(() => Drive.Data.Risk.Db.RiskApplications.FindAllBy(ApplicationId: application.Id,
			                                                                    PaymentTransactionCount: 1,
			                                                                    ManualPaymentTransactionCount: 0, //This is always going to be zero, the transaction source is not currently stored
			                                                                    MainCardRepayRate: 0).SingleOrDefault().ClosedOn != null);
		}

        [Test, AUT(AUT.Uk), Owner(Owner.RiskTeam)]
        public void L1ApplicationWillIncreaseTheRiskAccountRank()
        {
            var customer = CustomerBuilder.New().Build();
            var firstApplication = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            firstApplication.RepayOnDueDate();
            var accountRank = Drive.Data.Risk.Db.RiskAccounts.FindAllBy(AccountId:firstApplication.AccountId).SingleOrDefault().AccountRank;

            Assert.IsNotNull(accountRank,"The account rank should NOT be null");
            Assert.AreEqual(1,accountRank,"The account rank for L1 should be 1");
        }
	}
}
