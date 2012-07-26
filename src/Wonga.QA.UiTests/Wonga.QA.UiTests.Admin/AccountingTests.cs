using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Admin
{
    class AccountingTest : UiTest
    {
        /// <summary>
        /// Verifies canceled application 
        /// </summary>
        [Test, AUT(AUT.Uk), JIRA("UK-1754"), Pending("The test runs only in the Local environment. TODO: it should run in other environments too, i.e. WIP and RC."), Owner(Owner.RomanTertychnyi)]
        public void CancelV3Transaction()
        {
            Customer cust = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).Build();
            var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
            Assert.IsNotNull(db_app);
            var db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);
            Assert.IsNotNull(db_trans);

            var accountingPage = Client.Accounting().CashOut();
            accountingPage.SelectedTransactionType = "Unmatched Transactions V3 Records";
            accountingPage.FilterBy = "ApplicationId";
            accountingPage.FilterValue = String.Format("{0}", db_app.ExternalId);
            accountingPage.Search();
            accountingPage.GetSearchResults();
            accountingPage.MarkCancel(0);
            accountingPage.Update();
            db_trans = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(db_app.ApplicationId);

            Decimal Balance = 0;
            Do.With.Timeout(2).Until(
                () =>
                {
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
        [Test, AUT(AUT.Uk), JIRA("UK-1756"), Pending("The test runs only in the Local environment. TODO: it should run in other environments too, i.e. WIP and RC."), Owner(Owner.RomanTertychnyi)]
        public void SendPaymentV3Transactions()
        {
            Customer cust = CustomerBuilder.New().Build();
            Application app = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).Build();
            var db_app = Drive.Data.Payments.Db.Applications.FindByExternalId(app.Id);
            Assert.IsNotNull(db_app);

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
    }
}
