using System;
using System.Linq;
using System.Threading;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;

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
            FirstCollectionAttempt(null,false,false);
            SecondCollectionAttempt(false);
            Do.Until(this.IsInArrears);
            return this;
        }

        /// <summary>
        /// Triggers first collection attempt (5AM) and waits for it to either succeed or fail
        /// </summary>
        /// <param name="paymentPlan">payment plan</param>
        /// <param name="isFinalPayment">Specifies if we're collecting the final payment amount or a regular one</param>
        /// <param name="shouldSucceed">Specifies if we're expecting this collection attempt to succeed</param>
        public void FirstCollectionAttempt(PaymentPlanEntity paymentPlan, bool isFinalPayment, bool shouldSucceed)
        {
            if (!shouldSucceed)
            {
                SetCardExpirationDate(false);
            }
            else
            {
                SetCardExpirationDate(true);
            }

            CollectPaymentToday();

            if (shouldSucceed)
            {
                Do.Until(() => Drive.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == Id
                                                                           && t.Amount == (isFinalPayment
                                                                                ? paymentPlan.FinalAmount
                                                                                : paymentPlan.RegularAmount)
                                                                           && t.PostedOn.Date == DateTime.Today
                                                                           && t.Type == PaymentTransactionEnum.CardPayment.ToString()) > 0);
            }
            else
            {
                // Wait for the first attempt to fail
                Thread.Sleep(15000);
            }
        }

        private void CollectPaymentToday()
        {
            // Change the payment plan, o the collection starts today
            var paymentPlan = GetPaymentPlan();
            if (paymentPlan.StartDate > DateTime.Now)
            {
                paymentPlan.StartDate = paymentPlan.StartDate
                    .Add(DateTime.Now - paymentPlan.StartDate);
                paymentPlan.Submit();
            }

            var paymentSchedulingSaga =
                Drive.Db.OpsSagas.PaymentSchedulingSagaEntities.Single(
                    s => s.ApplicationExternalId == Id);

            Drive.Msmq.Payments.Send(new TimeoutMessage {SagaId = paymentSchedulingSaga.Id});
        }

        /// <summary>
        /// Triggers the second collection attempt, with an expected result, but does not wait for it to finish
        /// </summary>
        /// <param name="shouldSucceed"></param>
        public void SecondCollectionAttempt(bool shouldSucceed)
        {
            if (!shouldSucceed)
            {
                SetCardExpirationDate(false);
            }
            else
            {
                SetCardExpirationDate(true);
            }
            var businessLoansScheduledPaymentsSaga =
                Do.Until(() => Drive.Db.OpsSagas.BusinessLoanScheduledPaymentSagaEntities.Single(
                    s => s.ApplicationGuid == Id));

            // Trigger repeated collection
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = businessLoansScheduledPaymentsSaga.Id });
        }

        public void RestorePaymentCardExpiryDate()
        {
            SetCardExpirationDate(true);
        }

        private void SetCardExpirationDate(bool cardIsValid)
        {
            if ((!cardIsValid && _originalExpiryDate == null)
                || (cardIsValid && _originalExpiryDate != null))
            {
                var cardGuid =
                    Do.Until(
                        () => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.Single(
                            app => app.ApplicationEntity.ExternalId == Id).BusinessPaymentCardGuid);
                var cardEntity =
                    Do.Until(
                        () => Drive.Db.Payments.BusinessPaymentCards.Single(
                            c => c.PaymentCardsBaseEntity.ExternalId == cardGuid));

                if (!cardIsValid && _originalExpiryDate == null)
                {
                    _originalExpiryDate = cardEntity.PaymentCardsBaseEntity.ExpiryDate;
                    cardEntity.PaymentCardsBaseEntity.ExpiryDate = DateTime.Today.AddDays(-5);
                }

                if (cardIsValid && _originalExpiryDate != null)
                {
                    cardEntity.PaymentCardsBaseEntity.ExpiryDate = _originalExpiryDate.Value;
                    _originalExpiryDate = null;
                }

                cardEntity.Submit();
            }
        }

        public bool IsInArrears()
        {
            var accountId = Do.Until(() => Drive.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == Id).AccountId);
            var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
            {
                AccountId = accountId
            });

            var arrears = 0M;
            var arrearsString = response.Values["Arrears"].SingleOrDefault();
            if (!string.IsNullOrEmpty(arrearsString))
            {
                decimal.TryParse(arrearsString, out arrears);
            }
            return arrears > 0M;
        }

		public decimal GetArrearsAmount()
		{
			var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
			{
				AccountId = AccountId
			});
			return decimal.Parse(response.Values["Arrears"].Single());
		} 


        // Currently not working as business account summary does not include default charges
        ///// <summary>
        ///// Not working at the moment
        ///// </summary>
        ///// <returns></returns>
        //public bool HasOutstandingCharges()
        //{
        //    var accountId = Do.Until(() => Drive.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == Id).AccountId);
        //    var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
        //    {
        //        AccountId = accountId
        //    });

        //    var outstandingCharges = 0M;
        //    // TODO: change the field name
        //    var outstandingChargesString = response.Values["OutstandingFees"].SingleOrDefault();
        //    if (!string.IsNullOrEmpty(outstandingChargesString))
        //    {
        //        decimal.TryParse(outstandingChargesString, out outstandingCharges);
        //    }
        //    return outstandingCharges > 0M;
        //}

        public PaymentPlanEntity GetPaymentPlan()
        {
            return Do.Until(() => Drive.Db.Payments.PaymentPlans.Single(pp => pp.ApplicationEntity.ExternalId == Id && pp.CanceledOn == null));
        }
    }
}
