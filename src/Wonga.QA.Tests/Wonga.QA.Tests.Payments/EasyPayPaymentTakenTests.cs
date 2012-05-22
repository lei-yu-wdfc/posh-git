using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Za), Parallelizable(TestScope.All)]
    public class EasyPayPaymentTakenTests
    {
		//TODO chain these tests with dependsOn
        [Test, AUT(AUT.Za), JIRA("ZA-2395")]
        public void RepayUsingEasyPayWillCreateTransactionWhenPaymentReceived()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);

        	var repaymentNumber = GenerateEasyPayNumberWithCsApi(customer);

            Drive.Msmq.EasyPay.Send(new PaymentResponseDetailRecordZaCommand
                                            {
                                                ActionDate = DateTime.UtcNow.Date,
                                                Amount = 10M,
												RepaymentNumber = repaymentNumber,
                                                Filename = "TestFile"
                                            });

            //This will cause payment to handle event and create transaction.

            var transaction = Do.With.Until(() => Drive.Db.Payments.Transactions.Single
                (r => r.Amount == -10M && r.ApplicationId == app.ApplicationId));

            Assert.AreEqual("Payment from EasyPay", transaction.Reference);
        }

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), Pending]
        public void RepayUsingEasyPayFullRepaymentBeforeDueDateClosesApplication()
		{
			
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), Pending]
		public void RepayUsingEasyPayParialRepaymentBeforeDueDateDoesntCloseApplication()
		{

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), Pending]
		public void RepayUsingEasyPayFullRepaymentAfterDueDateClosesApplication()
		{

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2395"), Pending]
		public void RepayUsingEasyPayParialRepaymentAfterDueDateDoesntCloseApplication()
		{

		}

		#region Helpers

		private string GenerateEasyPayNumberWithCsApi(Customer customer)
		{
			var generateRepaymentNumberCommand = new Framework.Cs.GenerateRepaymentNumberCommand
			{
				AccountId = customer.Id
			};

			Drive.Cs.Commands.Post(generateRepaymentNumberCommand);

			dynamic repaymentAccounts = Drive.Data.Payments.Db.RepaymentAccount;
			var ra = Do.Until(() => repaymentAccounts.FindAll(repaymentAccounts.AccountId == customer.Id)
													.FirstOrDefault());

			Assert.IsNotNull(ra);
			Assert.IsNotNull(ra.RepaymentNumber);

			return ra.RepaymentNumber;
		}

		private string GenerateEasyPayNumberWithApi()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
