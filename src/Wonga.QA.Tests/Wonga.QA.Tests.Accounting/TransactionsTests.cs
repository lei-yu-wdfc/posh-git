using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Ui.Admin
{
    [TestFixture]
    public class MatchingTests : AdminTest
    {
        private Customer customer;
        private Organisation organisation;
        private Application application;

        [FixtureSetUp]
        public void InitFixture()
        {
            //customer = CustomerBuilder.New().Build();
            //organisation = OrganisationBuilder.New(customer).Build();
        }
        
        /// <summary>
        /// Verifies that payment transasction are handled by the accounting service
        /// </summary>
        [Test, AUT(AUT.Uk), JIRA("UK-1758")]
        public void SystemTransactionHandler()
        {
            Customer cust = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(10).Build();
            //var db_app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == app.Id);
            //var db_trans = Drive.Db.Payments.Transactions.Select(t => t.ApplicationId == db_app.ApplicationId);
            var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
            Assert.IsNotNull(db_app);
            var db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);
            Assert.IsNotNull(db_trans);
            foreach (var dbTran in db_trans)
            {
                Console.WriteLine("TransactionId: {0}", dbTran.TransactionId);
                var sysTran = Drive.Data.Accounting.Db.SystemTransactions.FindByTransactionId(dbTran.ExternalId);
                Assert.IsNotNull(sysTran);
                Assert.AreEqual(db_app.AccountId, sysTran.AccountId);
                Assert.AreEqual(dbTran.PostedOn, sysTran.PostedOn);
                Assert.AreEqual(dbTran.Scope, sysTran.Scope);
                Assert.AreEqual(dbTran.Amount, sysTran.Amount);
            }
        }

        /// <summary>
        /// Verifies repayed application balance
        /// </summary>
        [Ignore]
        [Test, AUT(AUT.Uk)]
        public void ClosedApplicationBalance()
        {
            Customer cust = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(10).Build();
            var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
            Assert.IsNotNull(db_app);
            var db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);
            Assert.IsNotNull(db_trans);
            app.RepayOnDueDate();
            db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);
            Decimal Balance = 0;
            foreach (var dbTran in db_trans)
            {
                Balance += dbTran.Amount;
            }
            Assert.AreEqual(0, Balance);
        }

        /// <summary>
        /// Verifies canceled application 
        /// </summary>
        [Test, AUT(AUT.Uk), JIRA("UK-1754")]
        public void CancelV3Transaction()
        {
            Customer cust = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).Build();
            var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
            Assert.IsNotNull(db_app);
            var db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);
            Assert.IsNotNull(db_trans);

            Client = new UiClient();
            var accountingPage = Client.Accounting().CashOut();
            accountingPage.SelectedTransactionType = "Unmatched Transactions V3 Records";
            accountingPage.FilterBy = "ApplicationId";
            accountingPage.FilterValue = String.Format("{0}", db_app.ExternalId);
            accountingPage.Search();
            accountingPage.GetSearchResults();
            accountingPage.MarkCancel(0);
            accountingPage.Update();
            Client.Dispose();
            db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);

            Decimal Balance = 0;
            Do.With.Timeout(2).Until(
                () => {
                    db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);
                    Balance = 0;
                    int transCount = 0;
                    foreach (var dbTran in db_trans)
                    {
                        transCount += 1;
                        Balance += dbTran.Amount;
                    }
                    return transCount > 2;
                }
            );
            Assert.AreEqual(0, Balance);
        }

        /// <summary>
        /// Verifies resend payment functionality
        /// </summary>
        [Test, AUT(AUT.Uk),JIRA("UK-1756")]
        public void SendPaymentV3Transactions()
        {
            Customer cust = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).Build();
            var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
            Assert.IsNotNull(db_app);

            Client = new UiClient();
            var accountingPage = Client.Accounting().CashOut();
            accountingPage.SelectedTransactionType = "Unmatched Transactions V3 Records";
            accountingPage.FilterBy = "ApplicationId";
            accountingPage.FilterValue = String.Format("{0}", db_app.ExternalId);
            accountingPage.Search();
            accountingPage.GetSearchResults();

            var db_system_trans = Drive.Data.Accounting.Db.SystemTransactions.FindByApplicationId(db_app.ExternalId);
            db_system_trans.ExternalReference = null;
            Drive.Data.Accounting.Db.SystemTransactions.Update(db_system_trans);

            accountingPage.MarkSendPayment(0);
            accountingPage.Update();
            Client.Dispose();

            Do.With.Timeout(2).Until(
                () =>
                {
                    db_system_trans = Drive.Data.Accounting.Db.SystemTransactions.FindByApplicationId(db_app.ExternalId);
                    return (db_system_trans.ExternalReference != null);
                }
            );
            Assert.IsNotNull(db_system_trans.ExternalReference);
            Assert.IsFalse(db_system_trans.PaymentStatus == 0);
        }

        //[Test, AUT(AUT.Uk)]
        //public void BouncedBankStatement()
        //{
        //    Customer cust = CustomerBuilder.New().Build();
        //    Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).Build();
        //    var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
        //    Assert.IsNotNull(db_app);
        //}

        //[Test, AUT(AUT.Uk)]
        //public void ReturnedBankStatement()
        //{

        //}

        //[Test, AUT(AUT.Uk)]
        //public void CashInBankStatement()
        //{
        //    var BankStatement = AddBankStatement();
        //}

        private static dynamic AddBankStatement()
        {
            //var bankStatement = new BankStatement
            //                        {
            //                            ExternalId: Guid.NewGuid(),
            //                            ExternalReference: "AUTO TEST",
            //                            AccountCurrency: 826,
            //                            TransactionAmount: 105.5
            //                        };
            //Drive.Data.Accounting.Db.BankStatementRecords.Insert(bankStatement);
            //return bankStatement;
            return 0;
        }
    }
}
