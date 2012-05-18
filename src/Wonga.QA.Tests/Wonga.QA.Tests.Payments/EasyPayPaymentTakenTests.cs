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
        [Test, AUT(AUT.Za), JIRA("ZA-2395")]
        public void RepayUsingEasyPayWillCreateTransactionWhenPaymentReceived()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            //Request to generate easypay number via csapi
            var generateRepaymentNumberCommand = new Framework.Cs.GenerateRepaymentNumberCommand
            {
                AccountId = customer.Id
            };

            //Act
            Drive.Cs.Commands.Post(generateRepaymentNumberCommand);

            //Assert
            dynamic repaymentAccount = Drive.Data.Payments.Db.RepaymentAccount;
            var ra = Do.Until(() => repaymentAccount.FindAll(repaymentAccount.AccountId == customer.Id)
                                                    .FirstOrDefault());
            Assert.IsNotNull(ra);
            Assert.IsNotNull(ra.RepaymentNumber);

            Drive.Msmq.EasyPay.Send(new PaymentResponseDetailRecordZaCommand
                                            {
                                                ActionDate = DateTime.UtcNow.Date,
                                                Amount = 10M,
                                                RepaymentNumber = ra.RepaymentNumber,
                                                Filename = "TestFile"
                                            });

            //This will cause payment to handle event and create transaction.

            var transaction = Do.With.Until(() => Drive.Db.Payments.Transactions.Single
                (r => r.Amount == -10M && r.ApplicationId == app.ApplicationId));

            Assert.AreEqual("Payment from EasyPay", transaction.Reference);
        }
    }
}
