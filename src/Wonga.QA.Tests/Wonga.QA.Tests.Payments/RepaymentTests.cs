using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Wb)]
    public class RepaymentTests
    {
        private BusinessApplication applicationInfo;
        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build() as BusinessApplication;
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 4
        /// </summary>
        [Test, JIRA("SME-1018", "SME-808", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenFirstCollectionAttemptSucceeds()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();

            applicationInfo.FirstCollectionAttempt(paymentPlan, false, true);
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 6
        /// </summary>
        [Test, JIRA("SME-1018", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenSecondCollectionAttemptSucceeds()
        {
            applicationInfo.GetPaymentPlan();

            applicationInfo.FirstCollectionAttempt(null, false, false);

            applicationInfo.SecondCollectionAttempt(true);

            // Check that only one transaction has occured
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-1018", "SME-812", "SME-809")]
        public void PaymentsShouldNotCreateNewTransactionWhenSecondCollectionAttemptFails()
        {
            applicationInfo.GetPaymentPlan();
            applicationInfo.FirstCollectionAttempt(null, false, false);

            applicationInfo.SecondCollectionAttempt(false);

            // Check we have a default charge and that no transactions have been written to DB
            Do.Until(() => Driver.Db.Payments.Transactions.SingleOrDefault(
                    t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                         && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
            Assert.IsNull(Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-808", "SME-809")]
        public void PaymentsShouldCreateRepaymentPlanWhenLoanIsApproved()
        {
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
            var paymentPlan = applicationInfo.GetPaymentPlan();

            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);

            var initialBalance = GetTotalOutstandingAmount(accountId);

            applicationInfo.FirstCollectionAttempt(paymentPlan, false, true);

            var balanceAfterTx = GetTotalOutstandingAmount(accountId);

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
            var paymentPlan = applicationInfo.GetPaymentPlan();

            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);

            var initialBalance = GetTotalOutstandingAmount(accountId);

            applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            applicationInfo.SecondCollectionAttempt(true);
            // Wait for he second payment to succeed
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Amount == paymentPlan.RegularAmount
                                                                    && t.Scope == (int)PaymentTransactionScopeEnum.Credit
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));

            var balanceAfterTx = GetTotalOutstandingAmount(accountId);

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
            var paymentPlan = applicationInfo.GetPaymentPlan();

            applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            applicationInfo.SecondCollectionAttempt(false);
            // Wait for he second payment to succeed
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                                                                    && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));

            MoveBackInTime(7, true);

            applicationInfo.FirstCollectionAttempt(paymentPlan, false, true);

            Do.Until(() => Driver.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                 &&
                                                                 t.Type == PaymentTransactionEnum.CardPayment.ToString()
                                                                 && t.Amount == paymentPlan.RegularAmount
                                                                 && t.Scope == (int)PaymentTransactionScopeEnum.Credit) == 2);

            Do.Until(() => Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                     &&
                                                     t.Type == PaymentTransactionEnum.CardPayment.ToString()
                                                     && t.Amount == 10
                                                     && t.Scope == (int)PaymentTransactionScopeEnum.Credit));
        }

        [Test, JIRA("SME-808")]
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfullWithOneFailedIntermediateCollection()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();
            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);
            MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                if (i == paymentPlan.NumberOfPayments - 2)
                {
                    applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
                    applicationInfo.SecondCollectionAttempt(false);
                    Thread.Sleep(15000);
                }
                else
                {
                    applicationInfo.FirstCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                }
                MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = GetTotalOutstandingAmount(accountId);

            Assert.AreEqual(0, totalOutstandingAmount);
        }

        [Test, JIRA("SME-808")]
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfull()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();
            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);
            MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                applicationInfo.FirstCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = GetTotalOutstandingAmount(accountId);

            Assert.AreEqual(0, totalOutstandingAmount);
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
            var paymentPlan = applicationInfo.GetPaymentPlan();
            applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            Driver.Msmq.Payments.Send(new CreateTransactionCommand
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
            applicationInfo.SecondCollectionAttempt(false);
            // Wait for collection atempt to fail
            Thread.Sleep(15000);

            Assert.IsNull(Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
        }

        private void MoveBackInTime(int days, bool movePaymentPlans)
        {
            days = -days;
            if (movePaymentPlans)
            {
                Do.Until(() =>
                             {
                                 var paymentPlans =
                                     Driver.Db.Payments.PaymentPlans.Where(
                                         pp => pp.ApplicationEntity.ExternalId == applicationInfo.Id);
                                 foreach (var paymentPlan in paymentPlans)
                                 {
                                     paymentPlan.StartDate = paymentPlan.StartDate.AddDays(days);
                                     paymentPlan.EndDate = paymentPlan.EndDate.AddDays(days);
                                     paymentPlan.CreatedOn = paymentPlan.CreatedOn.AddDays(days);
                                     if (paymentPlan.CanceledOn != null)
                                     {
                                         paymentPlan.CanceledOn = paymentPlan.CanceledOn.Value.AddDays(days);
                                     }
                                     paymentPlan.Submit();
                                 }
                                 return true;
                             });
            }

            var transactionEntities = Driver.Db.Payments.Transactions.Where(t => t.ApplicationEntity.ExternalId == applicationInfo.Id);
            foreach (var transaction in transactionEntities)
            {
                transaction.CreatedOn = transaction.CreatedOn.AddDays(days);
                transaction.PostedOn = transaction.PostedOn.AddDays(days);
                transaction.Submit();
            }
        }

        private static double GetTotalOutstandingAmount(Guid accountId)
        {
            var response = Driver.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
                                                        {
                                                            AccountId = accountId
                                                        });
            return double.Parse(response.Values["TotalOutstandingAmount"].Single());
        }
    }
}
