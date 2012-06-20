using System;
using System.Linq;
using System.Threading;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;
using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Payments.Csapi.Commands.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.FileStorage.InternalMessages.PaymentTransactionScopeEnum;

namespace Wonga.QA.Framework
{
    public class BusinessApplication : Application
    {
        public Guid MainApplicantId { get; set; }
        public Guid OrganisationId { get; set; }

        private DateTime? _originalExpiryDate;

        public BusinessApplication(Guid id, Guid mainApplicantId, Guid organisationId):base(id)
        {
            this.MainApplicantId = mainApplicantId;
            this.OrganisationId = organisationId;
        }

        public override Application PutIntoArrears()
        {
            // Try to collect the weekly amount twice and check that we've created a default charge transaction
            MorningCollectionAttempt(null,false,false);
            AfternoonCollectionAttempt(false);
            Do.Until(this.IsInArrears);
            return this;
        }

        /// <summary>
        /// Triggers first collection attempt (5AM) and waits for it to either succeed or fail
        /// </summary>
        /// <param name="paymentPlan">payment plan</param>
        /// <param name="isFinalPayment">Specifies if we're collecting the final payment amount or a regular one</param>
        /// <param name="shouldSucceed">Specifies if we're expecting this collection attempt to succeed</param>
        /// <param name="isSecondAttempt">if set to true, indicates that this is the collection attempt 48h after the initial one</param>
        public void MorningCollectionAttempt(PaymentPlanEntity paymentPlan, bool isFinalPayment, bool shouldSucceed, bool isSecondAttempt = false)
        {
            if (!shouldSucceed)
            {
                SetCardExpirationDate(false);
            }
            else
            {
                SetCardExpirationDate(true);
            }

            CollectPaymentToday(isSecondAttempt);

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

        private void CollectPaymentToday(bool isSecondAttempt)
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
                Do.Until(() => Drive.Data.OpsSagas.Db.PaymentSchedulingSagaEntity.FindByApplicationExternalId(Id));

            var message = new TimeoutMessage {SagaId = paymentSchedulingSaga.Id};
            if(isSecondAttempt)
            {
                message.State = 2;
            }

            Drive.Msmq.Payments.Send(message);
        }

        /// <summary>
        /// Triggers the second collection attempt, with an expected result, but does not wait for it to finish
        /// </summary>
        /// <param name="shouldSucceed"></param>
        /// <param name="isSecondAttempt">if set to true, indicates that this is the collection attempt 48h after the initial one</param>
        public void AfternoonCollectionAttempt(bool shouldSucceed)
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
                Do.With.Timeout(2).Until(() => Drive.Data.OpsSagas.Db.BusinessLoanScheduledPaymentSagaEntity.FindByApplicationGuid(Id));

            // Trigger repeated collection
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = businessLoansScheduledPaymentsSaga.Id});
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

        public override bool IsInArrears()
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

        /// <summary>
        /// Changes existing transaction dates as if they have appeared <paramref name="days"/> earlier. If <paramref name="movePaymentPlans"/> is true, moves the payment plan dates as well
        /// </summary>
        /// <param name="days">Number of days to move back</param>
        /// <param name="movePaymentPlans">True, if we need to move payment plans</param>
        public void MoveBackInTime(int days, bool movePaymentPlans)
        {
            if (days > 0)
            {
                days = -days;
            }
            if (movePaymentPlans)
            {
                Do.Until(() =>
                {
                    var paymentPlans =
                        Drive.Db.Payments.PaymentPlans.Where(
                            pp => pp.ApplicationEntity.ExternalId == Id);
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

            var transactionEntities =
                Drive.Db.Payments.Transactions.Where(t => t.ApplicationEntity.ExternalId == Id);
            foreach (var transaction in transactionEntities)
            {
                transaction.CreatedOn = transaction.CreatedOn.AddDays(days);
                transaction.PostedOn = transaction.PostedOn.AddDays(days);
                transaction.Submit();
            }
        }

        /// <summary>
        /// Creates a new transaction for the application with Scope=Credit and Type=Cheque
        /// </summary>
        /// <param name="extraPaymentAmount">Transaction amount</param>
        public void CreateExtraPayment(decimal extraPaymentAmount)
        {
            var command = new CreateTransactionCommand
            {
                Amount = extraPaymentAmount,
                ApplicationId = Id,
                Currency = CurrencyCodeIso4217Enum.GBP,
                ExternalId = Guid.NewGuid(),
                ComponentTransactionId = Guid.Empty,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Credit,
                Source = PaymentTransactionSourceEnum.System,
                Type = PaymentTransactionEnum.Cheque
            };

            Drive.Msmq.Payments.Send(command);
            Do.With.Timeout(2).Message("Transaction was not created").Until(
                () => Drive.Data.Payments.Db.Transations.FindExternalId(command.ExternalId));
            // Wait for the TX to be processed
            Thread.Sleep(15000);
        }

        public double GetTotalOutstandingAmount()
        {
            var response = Drive.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
            {
                AccountId = MainApplicantId
            });
            return double.Parse(response.Values["TotalOutstandingAmount"].Single());
        }
    }
}
