using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
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

			Drive.Msmq.Risk.Send(new IApplicationClosedEvent
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

		[Test, AUT(AUT.Uk), JIRA("WIN-1125")]
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
	}
}
