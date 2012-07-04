using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Wb)]
    [Parallelizable(TestScope.All)]
    public class RepaymentTests
    {

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 4
        /// </summary>
        [Test, JIRA("SME-1018", "SME-808", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenMorningCollectionAttemptSucceeds()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();

            applicationInfo.MorningCollectionAttempt(paymentPlan, false, true);
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 6
        /// </summary>
        [Test, JIRA("SME-1018", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenAfternoonCollectionAttemptSucceeds()
        {
            var applicationInfo = InitApplication();
            applicationInfo.GetPaymentPlan();

            applicationInfo.MorningCollectionAttempt(null, false, false);

            applicationInfo.AfternoonCollectionAttempt(true);

            // Check that only one transaction has occured
            Do.Until(
                () => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                 && t.Scope == (int) PaymentTransactionScopeEnum.Credit
                                                                 &&
                                                                 t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-1018", "SME-812", "SME-809")]
        public void PaymentsShouldNotCreateNewTransactionWhenAfternoonCollectionAttemptFails()
        {
            var applicationInfo = InitApplication();
            applicationInfo.GetPaymentPlan();
            applicationInfo.MorningCollectionAttempt(null, false, false);

            applicationInfo.AfternoonCollectionAttempt(false);

            // Check we have a default charge and that no transactions have been written to DB
            Do.Until(() => Drive.Db.Payments.Transactions.SingleOrDefault(
                t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                     && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
            Assert.IsNull(
                Drive.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                         && t.Scope == (int) PaymentTransactionScopeEnum.Credit
                         && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-808", "SME-809")]
        public void PaymentsShouldCreateRepaymentPlanWhenLoanIsApproved()
        {
            var applicationInfo = InitApplication();
            Assert.IsNotNull(applicationInfo.GetPaymentPlan());
        }

        /// <summary>
        /// Test 5 
        /// Given a main applicant has applied for a business loan 
        /// When the loan has been approved 
        /// When the repayment plan has been created 
        /// When a saga to collect the weekly repayment has been set up 
        /// When we collect the money on the due date each week 
        /// When the money collection is successful 
        /// Then update the account balance 
        /// </summary>
        [Test, JIRA("SME-808", "SME-809")]
        public void PaymentsShouldUpdateAccountBalanceWhenCollectionIsSuccessful()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();

            var initialBalance = applicationInfo.GetTotalOutstandingAmount();

            applicationInfo.MorningCollectionAttempt(paymentPlan, false, true);

            var balanceAfterTx = applicationInfo.GetTotalOutstandingAmount();

            Assert.LessThan(balanceAfterTx, initialBalance);
        }

        /// <summary>
        /// Test 7 
        /// Given a main applicant has applied for a business loan 
        /// When the loan has been approved 
        /// When the repayment plan has been created 
        /// When a saga to collect the weekly repayment has been set up 
        /// When we collect the money on the due date each week 
        /// When the money collection Fails 
        /// When we wait 12 hours and try to collect again 
        /// When the collection is successful 
        /// Then adjust the account balance 
        /// </summary>
        [Test, JIRA("SME-808")]
        public void PaymentsShouldUpdateAccountBalanceWhenSecondCollectionIsSuccessful()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();
            
            var initialBalance = applicationInfo.GetTotalOutstandingAmount();

            applicationInfo.MorningCollectionAttempt(paymentPlan, false, false);
            applicationInfo.AfternoonCollectionAttempt(true);
            // Wait for he second payment to succeed
            Do.Until(
                () => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                 && t.Amount == paymentPlan.RegularAmount
                                                                 && t.Scope == (int) PaymentTransactionScopeEnum.Credit
                                                                 &&
                                                                 t.Type == PaymentTransactionEnum.CardPayment.ToString()));

            var balanceAfterTx = applicationInfo.GetTotalOutstandingAmount();

            Assert.LessThan(balanceAfterTx, initialBalance);
        }

        /// <summary>
        /// Test 11 
        /// Given a main applicant has applied for a business loan 
        /// When the loan has been approved 
        /// When the repayment plan has been created 
        /// When a saga to collect the weekly repayment has been set up 
        /// When we collect the money on the due date each week 
        /// When the money collection Fails 
        /// When the 2nd collection also fails 
        /// When the next weekly collection is due 
        /// When the next weekly collection is successful 
        /// Then try to collect the outstanding fees 
        /// When succsessful 
        /// Then try to collect the outstandign arrears 
        /// </summary>
        [Test, JIRA("SME-808", "SME-812")]
        public void PaymentsShouldCollectFeesAndArrearsWhenNextCollectionIsSuccessful()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();

            applicationInfo.MorningCollectionAttempt(paymentPlan, false, false);
            applicationInfo.AfternoonCollectionAttempt(false);
            // Wait for he second payment to succeed
            Do.Until(
                () => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                 && t.Scope == (int) PaymentTransactionScopeEnum.Debit
                                                                 &&
                                                                 t.Type ==
                                                                 PaymentTransactionEnum.DefaultCharge.ToString()));

            applicationInfo.MoveBackInTime(7, true);

            applicationInfo.MorningCollectionAttempt(paymentPlan, false, true);

            Do.Until(
                () => Drive.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                &&
                                                                t.Type == PaymentTransactionEnum.CardPayment.ToString()
                                                                && t.Amount == paymentPlan.RegularAmount
                                                                && t.Scope == (int) PaymentTransactionScopeEnum.Credit) ==
                      2);

            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                         &&
                         t.Type == PaymentTransactionEnum.CardPayment.ToString()
                         && t.Amount == 10
                         && t.Scope == (int) PaymentTransactionScopeEnum.Credit));
        }

        [Test, JIRA("SME-808", "SME-1394")]
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfulWithOneFailedIntermediateCollection()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();
            
            var today = DateTime.UtcNow.Date;
            applicationInfo.MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                if (i == paymentPlan.NumberOfPayments - 2)
                {
                    applicationInfo.MorningCollectionAttempt(paymentPlan, false, false);
                    applicationInfo.AfternoonCollectionAttempt(false);
                    Thread.Sleep(15000);
                }
                else
                {
                    applicationInfo.MorningCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                }
                applicationInfo.MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(applicationInfo.Id).ClosedOn > today);
        }

        [Test, JIRA("SME-808", "SME-1394")]
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfull()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();
            
            var today = DateTime.UtcNow.Date;

            applicationInfo.MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                applicationInfo.MorningCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                applicationInfo.MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(applicationInfo.Id).ClosedOn > today);
        }


        [Test, JIRA("SME-1394")]
        public void BalanceShouldBeZeroAfterLoanWasFullyRepaidWithEarlyPayment()
        {
            var applicationInfo = InitApplication();
            var today = DateTime.UtcNow.Date;

            applicationInfo.MoveBackInTime(7, false);

            var totalOutstandingAmount = applicationInfo.GetTotalOutstandingAmount();

            applicationInfo.CreateExtraPayment((decimal)totalOutstandingAmount);

            totalOutstandingAmount = applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(applicationInfo.Id).ClosedOn > today);
        }

        /// <summary>
        /// Test 2 
        /// Given a loan has been advanced and a payment plan in place 
        /// When we try to collect a weekly payment 
        /// When this collection fails at both the 5am and again at the 5pm collection time 
        /// And overpayment has been made on the account to cover this payment 
        /// Then do not charge the account £10 for a failed collection 
        /// </summary>
        [Test, JIRA("SME-812")]
        public void DefaultChargeShouldNotBeCollectedWhenAnOverpaymentHasBeenMadeAndBothCollectionAttemptsFail()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();
            applicationInfo.MorningCollectionAttempt(paymentPlan, false, false);
            Drive.Msmq.Payments.Send(new CreateTransactionCommand
                                         {
                                             Amount = paymentPlan.RegularAmount,
                                             ApplicationId = applicationInfo.Id,
                                             Currency = CurrencyCodeIso4217Enum.GBP,
                                             ExternalId = Guid.NewGuid(),
                                             ComponentTransactionId = Guid.Empty,
                                             PostedOn = DateTime.Now,
                                             Scope = PaymentTransactionScopeEnum.Credit,
                                             Source = PaymentTransactionSourceEnum.System,
                                             Type = PaymentTransactionEnum.Cheque
                                         });

            // Wait for the TX to be processed
            Thread.Sleep(15000);
            applicationInfo.AfternoonCollectionAttempt(false);
            // Wait for collection atempt to fail
            Thread.Sleep(15000);

            Assert.IsNull(
                Drive.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                         && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
        }

        [Test, JIRA("SME-1039")]
        public void PaymentsShouldCollectMissedPayment48hAfterFailedWeeklyAttempt()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();
            applicationInfo.MorningCollectionAttempt(paymentPlan, false,false);
            applicationInfo.AfternoonCollectionAttempt(false);
            Thread.Sleep(1500);
            applicationInfo.MoveBackInTime(2, true);
            applicationInfo.MorningCollectionAttempt(paymentPlan, false, true, true);

            var appId = Drive.Data.Payments.Db.Applications.FindByExternalId(applicationInfo.Id).ApplicationId;

            Do.With.Message("Should collect only one time").Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appId, Type: PaymentTransactionEnum.CardPayment.ToString(), Amount: paymentPlan.RegularAmount).Count() == 1);
            Do.With.Message("Should contain one default charge transaction").Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appId, Type: PaymentTransactionEnum.DefaultCharge.ToString(), Amount: 10).Count() == 1);
            Do.With.Message("Should collect the missed payment fee").Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appId, Type: PaymentTransactionEnum.CardPayment.ToString(), Amount: 10).Count() == 1);
        }

        [Test, JIRA("SME-1039")]
        public void PaymentsShouldCollectMissedPaymentAndTwoDefaultChargesNextWeekWhenThe48hPaymentIsMissed()
        {
            var applicationInfo = InitApplication();
            var paymentPlan = applicationInfo.GetPaymentPlan();
            //Moving to first collection date
            applicationInfo.MoveBackInTime(7, true);
            // Failing both attempts
            applicationInfo.MorningCollectionAttempt(paymentPlan, false, false);
            applicationInfo.AfternoonCollectionAttempt(false);
            // Waiting for transactions to be created etc
            Thread.Sleep(1500);
            // Moving to 48h after first attempt
            applicationInfo.MoveBackInTime(2, true);
            // Failing both attempts after 48h
            applicationInfo.MorningCollectionAttempt(paymentPlan, false,false, true);
            applicationInfo.AfternoonCollectionAttempt(false);
            // Waiting for transactions to be created
            Thread.Sleep(1500);
            // Moving to second payday
            applicationInfo.MoveBackInTime(5, true);
            // Successfully collect everything
            applicationInfo.MorningCollectionAttempt(paymentPlan, false,true);

            var appId = Drive.Data.Payments.Db.Applications.FindByExternalId(applicationInfo.Id).ApplicationId;

            Do.With.Message("Should contain two collection transactions").Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appId, Type: PaymentTransactionEnum.CardPayment.ToString(), Amount: paymentPlan.RegularAmount).Count() == 2);
            Do.With.Message("Should contain two DefaultCharge transactions with amount equal to 10").Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appId, Type: PaymentTransactionEnum.DefaultCharge.ToString(), Amount: 10).Count() == 2);
            Do.With.Message("Should collect all fees in one transaction").Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appId, Type: PaymentTransactionEnum.CardPayment.ToString(), Amount: 20).Count() == 1);
        }

        private BusinessApplication InitApplication()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            BusinessApplication applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                                                      Build() as BusinessApplication;
            return applicationInfo;
        }
    }
}
