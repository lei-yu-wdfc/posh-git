using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Wb)]
    public class TakePaymentTests
    {
        [Test, JIRA("SME-1018")]
        public void PaymentsShoudlCreateNewTansactionWhenFirstCollectionAtemptSucceeds()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();

            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();
            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == applicationInfo.Id);

            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = paymentSchedulingSaga.Id });
            var paymentPlan = Do.Until(() => Driver.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == applicationInfo.Id));
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == applicationInfo.Id
                                                                    && t.Amount == paymentPlan.RegularAmount
                                                                    && t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        [Test, JIRA("SME-1018")]
        public void PaymentsShoudlCreateNewTansactionWhenSecondCollectionAtemptSucceeds()
        {
            // Start application
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == applicationInfo.Id);

            // Change amount to 13, which will be refused by the mock
            var paymentPlan = Do.Until(() => Driver.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == applicationInfo.Id));
            paymentPlan.RegularAmount = 13;
            paymentPlan.Submit();

            // Start collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = paymentSchedulingSaga.Id });

            // Wait for the first attempt to fail
            Do.Sleep(30);

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

        [Test, JIRA("SME-1018"), Pending("DefaultCharge transaction not being created yet")]
        public void PaymentsShoudlNowCreateNewTansactionWhenSecondCollectionAtemptFails()
        {
            // Setup application
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == applicationInfo.Id);

            // Change the amout to 13, which will be refused by the mock
            var paymentPlan = Do.Until(() => Driver.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == applicationInfo.Id));
            paymentPlan.RegularAmount = 13;
            paymentPlan.Submit();

            // Start collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = paymentSchedulingSaga.Id });

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
    }
}
