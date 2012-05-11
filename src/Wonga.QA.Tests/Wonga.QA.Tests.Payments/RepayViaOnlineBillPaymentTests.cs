using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Ca), Parallelizable(TestScope.All)]
	public class RepayViaOnlineBillPaymentTests
	{
		[Test, AUT(AUT.Ca), JIRA("CA-1441")]
		public void RepayViaOnlineBillPayment_ShouldCloseLoanWhenPaidInFull()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var accountPreferences = Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == customer.Id));

		    decimal repaymentAmount = application.LoanAmount;

			var cmd = new RepayLoanInternalViaOnlineBillPaymentCommand
			                                                   	{
			                                                   		PaymentDate = DateTime.UtcNow,
																	Ccin = accountPreferences.Ccin,
																	Amount = repaymentAmount,
																	CustomerFullName = "PayorName",
																	RemittanceTraceNumber = "TraceNumber"
			                                                   	};

			Drive.Msmq.Payments.Send(cmd);

			Do.With.Timeout(60).Until(() => application.IsClosed);
			VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(application.Id, -repaymentAmount);
		}
	}
}
