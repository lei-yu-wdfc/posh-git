using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Uk)]
    [Parallelizable(TestScope.All)]
    public class ChaseLoanInArrearsTests
    {
        private static readonly dynamic ChaseLoanInArrearsSagaEntities = Drive.Data.OpsSagas.Db.ChaseLoanInArrearsSagaEntity;
        private Customer _customer;
        private Application _application;
        private Application _secondApplication;
        private dynamic _sagaEntityFiveAm;
        private dynamic _sagaSecondEntityFail;
        private dynamic _sagaEntityFail;
        private static dynamic _primaryPaymentCardId;
        private int _appInternalId;
        private int _secondAppInternalId;
        private const int FiveAmState = 0;
        private const int EightAmState = 1;


        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldCreateNewTransactionWhenFiveAmCollectionSucceeds()
        {
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            _sagaEntityFiveAm = GetSagaEntity(appInternalId);
            SaagaTimeOutMessageBy5Or8Am(_sagaEntityFiveAm.Id, FiveAmState);
            PaymentTransactionForArrearPing(application, appInternalId);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsShouldCreateNewTransactionWhenFiveAmCollectionSucceeds"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenFiveAmCollectionSucceeds()
        {
            CheckChaseForArrearSagaIsComplete(_sagaEntityFiveAm.Id);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFirstPingFails()
        {
            _customer = GetCustomer();
            _application = GetApplicationInArrears(_customer);
            _primaryPaymentCardId =
                Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_application.AccountId).SingleOrDefault().
                    PrimaryPaymentCardId;
            _appInternalId = ApplicationOperations.GetAppInternalId(_application);
            SetCardExpiryDate(_customer.GetPaymentCard(), DateTime.Now.AddYears(-1)); // original card must be expired
            _customer.AddBadCard();
            _sagaEntityFail = GetSagaEntity(_appInternalId);
            SaagaTimeOutMessageBy5Or8Am(_sagaEntityFail.Id, FiveAmState);
            CheckPaymentRequestDeclinedTransaction(_application, _appInternalId);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFirstPingFails"), Owner(Owner.JonHurd)]
        public void PaymentsShouldCreateNewTransactionWhenEightAmCollectionSucceedsAfterFivePingFails()
        {

            UpdatePrimaryCard(_application.AccountId);
            SetCardExpiryDate(_customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            SaagaTimeOutMessageBy5Or8Am(_sagaEntityFail.Id, EightAmState);
            PaymentTransactionForArrearPing(_application, _appInternalId);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsShouldCreateNewTransactionWhenEightAmCollectionSucceedsAfterFivePingFails"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenEightAmCollectionSucceedsAfterFirstPingFail()
        {
            CheckChaseForArrearSagaIsComplete(_sagaEntityFail.Id);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsFirstPingFailsToMakeSecondPingAlsoFail()
        {
            var customer = GetCustomer();
            _secondApplication = GetApplicationInArrears(customer);
            _secondAppInternalId = ApplicationOperations.GetAppInternalId(_secondApplication);
            SetCardExpiryDate(customer.GetPaymentCard(),DateTime.Now.AddMonths(-3));
            customer.AddBadCard();
            _sagaSecondEntityFail = GetSagaEntity(_secondAppInternalId);
            SaagaTimeOutMessageBy5Or8Am(_sagaSecondEntityFail.Id, FiveAmState);
            var amountDue = _secondApplication.GetDueDateBalance();
            Do.With.Timeout(1).Until<bool>(
                () => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: _secondAppInternalId,
                                                                                    Amount: amountDue,
                                                                                    StatusDescription: "Request Declined"
                          ).Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsFirstPingFailsToMakeSecondPingAlsoFail"), Owner(Owner.JonHurd)]
        public void PaymentsRepaymentRequestSecondPingFailsAfterFirstPingFailed()
        {

            PaymentsFirstPingFailsToMakeSecondPingAlsoFail();
            var amountDue = _secondApplication.GetDueDateBalance();
            _sagaSecondEntityFail = GetSagaEntity(_secondAppInternalId);
            SaagaTimeOutMessageBy5Or8Am(_sagaSecondEntityFail.Id, EightAmState);
            amountDue = _secondApplication.GetDueDateBalance();
            Do.Until<bool>(
                () => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: _secondAppInternalId,
                                                                                    Amount: amountDue,
                                                                                    StatusDescription: "Request Declined"
                          ).Count() == 2);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsRepaymentRequestSecondPingFailsAfterFirstPingFailed"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenFirstAndSecondPingFails()
        {
            CheckChaseForArrearSagaIsComplete(_sagaSecondEntityFail.Id);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenItReceivesATimeoutForAPingThatDoesNotExist()
        {
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            var sagaEntity = GetSagaEntity(appInternalId);
            SaagaTimeOutMessageBy5Or8Am(sagaEntity.Id, 2);
            CheckChaseForArrearSagaIsComplete(sagaEntity.Id);
        }



        #region pending test
        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd), Pending("Ticket Not Implemented")]
        public void PaymentsShouldSuppressFiveAmPingIfSuppressionsAreActive()
        {
            // To Do
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd), Pending("Ticket Not Implemented")]
        public void PaymentsShouldSuppressEightAmPingIfSuppressionsAreActive()
        {
            // To Do .. not implemented yet
        }
        #endregion

        #region Helpers
        private static void SetCardExpiryDate(Guid card, DateTime expiryDate)
        {
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: card, ExpiryDate: expiryDate, DeactivatedOn: null);
        }

        private static void UpdatePrimaryCard(Guid id)
        {
            Drive.Data.Payments.Db.AccountPreferences.UpdateByAccountId(AccountId: id,
                                                                        PrimaryPaymentCardId: _primaryPaymentCardId);
        }

        private static Application GetApplicationInArrears(Customer customer)
        {
            const decimal loanAmount = 100;
            var application = ApplicationBuilder.New(customer)
                .WithLoanAmount(loanAmount)
                .WithLoanTerm(7)
                .Build();
            application.PutIntoArrears();

            return application;
        }

        private static Customer GetCustomer()
        {
            var customer = CustomerBuilder.New().Build();
            return customer;
        }

        private static void CheckPaymentRequestDeclinedTransaction(Application application, int appInternalId)
        {
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(
                () => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: appInternalId,
                                                                                    Amount: amountDue,
                                                                                    StatusDescription: "Request Declined"
                          ).Count() == 1);
        }

        public static void PaymentTransactionForArrearPing(Application application, int appInternalId)
        {
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appInternalId,
                                                                                               Type:
                                                                                                   PaymentTransactionEnum.
                                                                                                   CardPayment.ToString(),
                                                                                               Amount: amountDue * -1,
                                                                                               Scope:
                                                                                                   (int)
                                                                                                   PaymentTransactionScopeEnum.
                                                                                                       Credit,
                                                                                               Reference:
                                                                                                   "Automatic Ping (Card)").
                                                     Count() == 1);
        }

        private static dynamic GetSagaEntity(int appInternalId)
        {
            return Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());
        }

        private static void SaagaTimeOutMessageBy5Or8Am(dynamic saagaId, int fiveAmEightAmState)
        {
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = saagaId, State = fiveAmEightAmState });
        }

        private static void CheckChaseForArrearSagaIsComplete(dynamic sagaId)
        {
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById((Guid)sagaId) == null);
        }
        #endregion

    }

    public static class CustomerExtensionMethods
    {
        public static Customer AddBadCard(this Customer customer)
        {
            const string cardType = "visa";
            const string cardNumber = "9999888877776666";
            customer.AddPaymentCard(cardType, cardNumber, "777", DateTime.Now.AddMonths(3), true);
            return customer;
        }

        public static Customer AddGoodCard(this Customer customer)
        {
            const string cardType = "visa";
            const string cardNumber = "4444333322221111";
            customer.AddPaymentCard(cardType, cardNumber, "666", DateTime.Now.AddMonths(3), true);
            return customer;
        }
    }
}
