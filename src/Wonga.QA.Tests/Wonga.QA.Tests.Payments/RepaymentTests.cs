using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Wb)]
    public class RepaymentTests
    {
        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 4
        /// </summary>
        [Test, JIRA("SME-1018"), JIRA("SME-808")]
        public void PaymentsShouldCreateNewTransacionWhenFirstCollectionAttemptSucceeds()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();

            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            var paymentPlan = GetPaymentPlan(applicationInfo);

            SuccessfullyCollectMoney(paymentPlan, applicationInfo);
        }

        private static void SuccessfullyCollectMoney(PaymentPlanEntity paymentPlan, Application applicationInfo)
        {
            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == applicationInfo.Id);

            Driver.Msmq.Payments.Send(new TimeoutMessage {SagaId = paymentSchedulingSaga.Id});

            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                       && t.Amount == paymentPlan.RegularAmount
                                                                       &&
                                                                       t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        private static PaymentPlanEntity GetPaymentPlan(Application applicationInfo)
        {
            return Do.Until(() => Driver.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == applicationInfo.Id));
        }

        /// <summary>
        /// http://jira.wonga.com/browse/SME-808
        /// SME-808 test 6
        /// </summary>
        [Test, JIRA("SME-1018")]
        public void PaymentsShouldCreateNewTransacionWhenSecondCollectionAttemptSucceeds()
        {
            // Start application
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            var paymentPlan = GetPaymentPlan(applicationInfo);

            PaymentSchedulingSagaEntity paymentSchedulingSaga;

            FailToCollectMoney(paymentPlan, applicationInfo);

            // Change amount back to 700, both for the payent plan and both sagas
            var businessLoansScheduledPaymentsSaga =
                Do.Until(() => Driver.Db.OpsSagas.BusinessLoanScheduledPaymentSagaEntities.Single(
                    s => s.ApplicationGuid == applicationInfo.Id));

            businessLoansScheduledPaymentsSaga.Amount = 700;
            businessLoansScheduledPaymentsSaga.Submit();
            paymentPlan.RegularAmount = 700;
            paymentPlan.Submit();

            paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == applicationInfo.Id);

            paymentSchedulingSaga.RequestedAmount = 700;
            paymentSchedulingSaga.Submit();

            // Initialise repeated collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = businessLoansScheduledPaymentsSaga.Id});
            
            // Check that only one transaction has occured
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Amount == 700
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        private static void FailToCollectMoney(PaymentPlanEntity paymentPlan, Application applicationInfo)
        {
            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == applicationInfo.Id);

            // Change amount to 13, which will be refused by the mock
            paymentPlan.RegularAmount = 13;
            paymentPlan.Submit();

            // Start collection
            Driver.Msmq.Payments.Send(new TimeoutMessage {SagaId = paymentSchedulingSaga.Id});

            // Wait for the first attempt to fail
            Do.Sleep(30);
        }

        [Test, JIRA("SME-1018"), Pending("DefaultCharge transaction not being created yet")]
        public void PaymentsShouldNowCreateNewTransacionWhenSecondCollectionAttemptFails()
        {
            // Setup application
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            // Change the amout to 13, which will be refused by the mock
            var paymentPlan = GetPaymentPlan(applicationInfo);
            FailToCollectMoney(paymentPlan, applicationInfo);

            var businessLoansScheduledPaymentsSaga =
                Do.Until(() => Driver.Db.OpsSagas.BusinessLoanScheduledPaymentSagaEntities.Single(
                    s => s.ApplicationGuid == applicationInfo.Id));

            // Trigger repeated collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = businessLoansScheduledPaymentsSaga.Id });

            // Check that no transactions have been written to DB
            Do.Sleep(30);
            Assert.IsNull(Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
            Assert.IsNotNull(Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
        }

        [Test, JIRA("SME-808")]
        public void PaymentsShouldCreateRepayentPlanWhenLoanIsApproved()
        {
            // Setup application
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            GetPaymentPlan(applicationInfo);
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
        [Test, JIRA("SME-808")]
        public void PaymentsShouldUpdateAccountBalnaceWhenCollectionIsSuccessful()
        {
            // Setup application
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            var paymentPlan = GetPaymentPlan(applicationInfo);

            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == applicationInfo.Id).AccountId);

            var initialBalance = GetTotalOutstandingAmount(accountId);

            SuccessfullyCollectMoney(paymentPlan,applicationInfo);

            var balanceAfterTx = GetTotalOutstandingAmount(accountId);

            Assert.LessThan(balanceAfterTx,initialBalance);
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
