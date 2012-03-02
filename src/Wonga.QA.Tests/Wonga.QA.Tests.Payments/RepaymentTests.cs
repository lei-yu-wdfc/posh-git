using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
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
        [Test, JIRA("SME-808"), Pending("Paymests are first collecting fees and then arrears and overpayments are first allocated to arrears, so collected fee reduces arrears.")]
        public void PaymentsShouldCollectFeesAndArrearsWhenNextCollectionIsSuccessful()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();

            applicationInfo.FirstCollectionAttempt(paymentPlan, false, false);
            applicationInfo.SecondCollectionAttempt(false);
            // Wait for he second payment to succeed
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Scope == (int)PaymentTransactionScopeEnum.Debit
                                                                    && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));

            MoveBackInTime(7,true);

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
        public void BalanceShouldBeZeroAfterAllCollectionAttemptsAreSuccessfull()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();
            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);
            MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                applicationInfo.FirstCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                MoveBackInTime(7,true);
            }

            var totalOutstandingAmount = GetTotalOutstandingAmount(accountId);

            Assert.AreEqual(0, totalOutstandingAmount);
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
