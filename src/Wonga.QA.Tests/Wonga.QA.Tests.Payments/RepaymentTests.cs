using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Wb)]
    public class RepaymentTests
    {
        private BusinessApplication _applicationInfo;

        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            _applicationInfo =
                ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                    Build() as BusinessApplication;
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 4
        /// </summary>
        [Test, JIRA("SME-1018", "SME-808", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenFirstCollectionAttemptSucceeds()
        {
            var paymentPlan = _applicationInfo.GetPaymentPlan();

            _applicationInfo.FirstCollectionAttempt(paymentPlan, false, true);
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 6
        /// </summary>
        [Test, JIRA("SME-1018", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenSecondCollectionAttemptSucceeds()
        {
            _applicationInfo.GetPaymentPlan();

            _applicationInfo.FirstCollectionAttempt(null, false, false);

            _applicationInfo.SecondCollectionAttempt(true);

            // Check that only one transaction has occured
            Do.Until(
                () => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                                                                 && t.Scope == (int) PaymentTransactionScopeEnum.Credit
                                                                 &&
                                                                 t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-1018", "SME-812", "SME-809")]
        public void PaymentsShouldNotCreateNewTransactionWhenSecondCollectionAttemptFails()
        {
            _applicationInfo.GetPaymentPlan();
            _applicationInfo.FirstCollectionAttempt(null, false, false);

            _applicationInfo.SecondCollectionAttempt(false);

            // Check we have a default charge and that no transactions have been written to DB
            Do.Until(() => Drive.Db.Payments.Transactions.SingleOrDefault(
                t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                     && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
            Assert.IsNull(
                Drive.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                         && t.Scope == (int) PaymentTransactionScopeEnum.Credit
                         && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-808", "SME-809")]
        public void PaymentsShouldCreateRepaymentPlanWhenLoanIsApproved()
        {
            Assert.IsNotNull(_applicationInfo.GetPaymentPlan());
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
            var paymentPlan = _applicationInfo.GetPaymentPlan();

            var initialBalance = _applicationInfo.GetTotalOutstandingAmount();

            _applicationInfo.FirstCollectionAttempt(paymentPlan, false, true);

            var balanceAfterTx = _applicationInfo.GetTotalOutstandingAmount();

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
            var paymentPlan = _applicationInfo.GetPaymentPlan();
            
            var initialBalance = _applicationInfo.GetTotalOutstandingAmount();

            _applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            _applicationInfo.SecondCollectionAttempt(true);
            // Wait for he second payment to succeed
            Do.Until(
                () => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                                                                 && t.Amount == paymentPlan.RegularAmount
                                                                 && t.Scope == (int) PaymentTransactionScopeEnum.Credit
                                                                 &&
                                                                 t.Type == PaymentTransactionEnum.CardPayment.ToString()));

            var balanceAfterTx = _applicationInfo.GetTotalOutstandingAmount();

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
            var paymentPlan = _applicationInfo.GetPaymentPlan();

            _applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            _applicationInfo.SecondCollectionAttempt(false);
            // Wait for he second payment to succeed
            Do.Until(
                () => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                                                                 && t.Scope == (int) PaymentTransactionScopeEnum.Debit
                                                                 &&
                                                                 t.Type ==
                                                                 PaymentTransactionEnum.DefaultCharge.ToString()));

            _applicationInfo.MoveBackInTime(7, true);

            _applicationInfo.FirstCollectionAttempt(paymentPlan, false, true);

            Do.Until(
                () => Drive.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                                                                &&
                                                                t.Type == PaymentTransactionEnum.CardPayment.ToString()
                                                                && t.Amount == paymentPlan.RegularAmount
                                                                && t.Scope == (int) PaymentTransactionScopeEnum.Credit) ==
                      2);

            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                         &&
                         t.Type == PaymentTransactionEnum.CardPayment.ToString()
                         && t.Amount == 10
                         && t.Scope == (int) PaymentTransactionScopeEnum.Credit));
        }

        [Test, JIRA("SME-808", "SME-1394")]
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfulWithOneFailedIntermediateCollection()
        {
            var paymentPlan = _applicationInfo.GetPaymentPlan();
            
            var today = DateTime.UtcNow.Date;
            _applicationInfo.MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                if (i == paymentPlan.NumberOfPayments - 2)
                {
                    _applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
                    _applicationInfo.SecondCollectionAttempt(false);
                    Thread.Sleep(15000);
                }
                else
                {
                    _applicationInfo.FirstCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                }
                _applicationInfo.MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = _applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(_applicationInfo.Id).ClosedOn > today);
        }

        [Test, JIRA("SME-808", "SME-1394")]
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfull()
        {
            var paymentPlan = _applicationInfo.GetPaymentPlan();
            
            var today = DateTime.UtcNow.Date;

            _applicationInfo.MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                _applicationInfo.FirstCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                _applicationInfo.MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = _applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(_applicationInfo.Id).ClosedOn > today);
        }


        [Test, JIRA("SME-1394")]
        public void BalanceShouldBeZeroAfterLoanWasFullyRepaidWithEarlyPayment()
        {
            var today = DateTime.UtcNow.Date;

            _applicationInfo.MoveBackInTime(7, false);

            var totalOutstandingAmount = _applicationInfo.GetTotalOutstandingAmount();

            _applicationInfo.CreateExtraPayment((decimal)totalOutstandingAmount);

            totalOutstandingAmount = _applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(_applicationInfo.Id).ClosedOn > today);
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
            var paymentPlan = _applicationInfo.GetPaymentPlan();
            _applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            Drive.Msmq.Payments.Send(new CreateTransactionCommand
                                         {
                                             Amount = paymentPlan.RegularAmount,
                                             ApplicationId = _applicationInfo.Id,
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
            _applicationInfo.SecondCollectionAttempt(false);
            // Wait for collection atempt to fail
            Thread.Sleep(15000);

            Assert.IsNull(
                Drive.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == _applicationInfo.Id
                         && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
        }
    }
}
