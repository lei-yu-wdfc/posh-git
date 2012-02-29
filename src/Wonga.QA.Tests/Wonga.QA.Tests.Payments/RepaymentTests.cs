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
        [Test, JIRA("SME-1018","SME-808", "SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenFirstCollectionAttemptSucceeds()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();

            applicationInfo.SuccessfullyCollectMoney(paymentPlan);
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 6
        /// </summary>
        [Test, JIRA("SME-1018","SME-809")]
        public void PaymentsShouldCreateNewTransactionWhenSecondCollectionAttemptSucceeds()
        {
            applicationInfo.GetPaymentPlan();

            applicationInfo.FailToCollectMoney();

            var businessLoansScheduledPaymentsSaga =
                Do.Until(() => Driver.Db.OpsSagas.BusinessLoanScheduledPaymentSagaEntities.Single(
                    s => s.ApplicationGuid == applicationInfo.Id));

            // Initialise repeated collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = businessLoansScheduledPaymentsSaga.Id });

            // Check that only one transaction has occured
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Amount == 700
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-1018","SME-812","SME-809")]
        public void PaymentsShouldNotCreateNewTransactionWhenSecondCollectionAttemptFails()
        {
            applicationInfo.GetPaymentPlan();
            applicationInfo.FailToCollectMoney(false);

            applicationInfo.SecondCollectionAttempt();

            // Check that no transactions have been written to DB
            Thread.Sleep(15000);
            Assert.IsNull(Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
            Assert.IsNotNull(Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
        }

        [Test, JIRA("SME-808","SME-809")]
        public void PaymentsShouldCreateRepaymentPlanWhenLoanIsApproved()
        {
            applicationInfo.GetPaymentPlan();
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
        [Test, JIRA("SME-808","SME-809")]
        public void PaymentsShouldUpdateAccountBalanceWhenCollectionIsSuccessful()
        {
            var paymentPlan = applicationInfo.GetPaymentPlan();

            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);

            var initialBalance = GetTotalOutstandingAmount(accountId);

            applicationInfo.SuccessfullyCollectMoney(paymentPlan);

            var balanceAfterTx = GetTotalOutstandingAmount(accountId);

            Assert.LessThan(balanceAfterTx, initialBalance);
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
