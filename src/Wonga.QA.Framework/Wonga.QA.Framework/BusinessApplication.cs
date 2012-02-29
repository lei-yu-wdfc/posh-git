using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;

namespace Wonga.QA.Framework
{
    public class BusinessApplication : Application
    {
        private DateTime? _originalExpiryDate;

        public BusinessApplication(Guid id):base(id)
        {
            
        }

        public override Application PutApplicationIntoArrears()
        {
            // Try to collect the weekly amount twice and check that we've created a default charge transaction
            FailToCollectMoney(false);
            SecondCollectionAttempt();
            Do.While(() => Driver.Db.Payments.Transactions.SingleOrDefault(t => t.ApplicationEntity.ExternalId == Id
                                                        && t.Type == PaymentTransactionEnum.DefaultCharge.ToString()));
            return this;
        }

        public void SuccessfullyCollectMoney(PaymentPlanEntity paymentPlan)
        {
            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == Id);

            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = paymentSchedulingSaga.Id });

            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == Id
                                                                       && t.Amount == paymentPlan.RegularAmount
                                                                       &&
                                                                       t.Type == PaymentTransactionEnum.CardPayment.ToString()));
        }

        public void SecondCollectionAttempt()
        {
            var businessLoansScheduledPaymentsSaga =
                Do.Until(() => Driver.Db.OpsSagas.BusinessLoanScheduledPaymentSagaEntities.Single(
                    s => s.ApplicationGuid == Id));

            // Trigger repeated collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = businessLoansScheduledPaymentsSaga.Id });
        }

        public void FailToCollectMoney(bool restoreCardData = true)
        {
            var paymentSchedulingSaga =
                Driver.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == Id);

            // Change CV2 of the card to 666, that wil fail the card payment

            var cardGuid = Do.Until(() => Driver.Db.Payments.BusinessFixedInstallmentLoanApplications.Single(app => app.ApplicationEntity.ExternalId == Id).BusinessPaymentCardGuid);
            var cardEntity = Do.Until(() => Driver.Db.Payments.BusinessPaymentCards.Single(c => c.PaymentCardsBaseEntity.ExternalId == cardGuid));
            _originalExpiryDate = cardEntity.PaymentCardsBaseEntity.ExpiryDate;
            cardEntity.PaymentCardsBaseEntity.ExpiryDate = DateTime.Today.AddDays(-5);
            cardEntity.Submit();
            // Start collection
            Driver.Msmq.Payments.Send(new TimeoutMessage { SagaId = paymentSchedulingSaga.Id });

            // Wait for the first attempt to fail
            Do.Sleep(15);
            if (restoreCardData)
            {
                cardEntity.PaymentCardsBaseEntity.ExpiryDate = _originalExpiryDate.Value;
                cardEntity.Submit();
                _originalExpiryDate = null;
            }
        }

        public void MakeFurtherCollecionsSucceed()
        {
            if (_originalExpiryDate.HasValue)
            {
                var cardGuid = Do.Until(() => Driver.Db.Payments.BusinessFixedInstallmentLoanApplications.Single(app => app.ApplicationEntity.ExternalId == Id).BusinessPaymentCardGuid);
                var cardEntity = Do.Until(() => Driver.Db.Payments.BusinessPaymentCards.Single(c => c.PaymentCardsBaseEntity.ExternalId == cardGuid));
                cardEntity.PaymentCardsBaseEntity.ExpiryDate = _originalExpiryDate.Value;
                cardEntity.Submit();
                _originalExpiryDate = null;
            }
        }
    }
}
