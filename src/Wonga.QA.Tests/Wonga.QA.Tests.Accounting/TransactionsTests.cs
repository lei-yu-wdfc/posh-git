using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Ui.Admin
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class MatchingTests
    {
        //private Customer customer;
        //private Organisation organisation;
        //private Application application;

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
        [Test, AUT(AUT.Uk), Owner(Owner.RomanTertychnyi)]
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
