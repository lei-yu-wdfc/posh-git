using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
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
        private decimal _amountDue;
        private int _appInternalId;
        private int _secondAppInternalId;

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldCreateNewTransactionWhenFiveAmCollectionSucceeds()
        {
            // Arrange
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);

            _sagaEntityFiveAm = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Call timeout with a state of 0 representing the first ping at 5 am ping
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = _sagaEntityFiveAm.Id, State = 0 });

            // Assert there should be a credit transaction in the table representing the payment
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appInternalId,
                                                                                Type: PaymentTransactionEnum.CardPayment.ToString(),
                                                                                Amount: amountDue * -1,
                                                                                Scope: (int)PaymentTransactionScopeEnum.Credit,
                                                                                Reference: "Automatic Ping (Card)").Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsShouldCreateNewTransactionWhenFiveAmCollectionSucceeds"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenFiveAmCollectionSucceeds()
        {
            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(_sagaEntityFiveAm.Id) == null);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFirstPingFails()
        {
            // Arrange
            _customer = GetCustomer();
            _application = GetApplicationInArrears(_customer);
            _primaryPaymentCardId =
                Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_application.AccountId).SingleOrDefault().
                    PrimaryPaymentCardId;
            _appInternalId = ApplicationOperations.GetAppInternalId(_application);

            _customer.AddBadCard();
            _sagaEntityFail = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == _appInternalId).Single());

            // Call timeout with a state of 0 representing the first ping at 5 am ping
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = _sagaEntityFail.Id, State = 0 });

            // Assert there should be a credit transaction in the table representing the payment
           _amountDue = _application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: _appInternalId,
                                                                                Amount: _amountDue,
                                                                                StatusDescription: "Request Declined"
                                                                                 ).Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFirstPingFails"), Owner(Owner.JonHurd)]
        public void PaymentsShouldCreateNewTransactionWhenEightAmCollectionSucceedsAfterFivePingFails()
        {
            UpdatePrimaryCard(_application.AccountId);
            SetCardExpiryDate(_customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            // Call timeout with a state of 1 representing the second ping at 8 am ping
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = _sagaEntityFail.Id, State = 1 });

            // Assert there should be a credit transaction in the table representing the payment
            
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: _appInternalId,
                                                                                Type: PaymentTransactionEnum.CardPayment.ToString(),
                                                                                Amount: _amountDue * -1,
                                                                                Scope: (int)PaymentTransactionScopeEnum.Credit,
                                                                                Reference: "Automatic Ping (Card)").Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsShouldCreateNewTransactionWhenEightAmCollectionSucceedsAfterFivePingFails"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenEightAmCollectionSucceedsAfterFirstPingFail()
        {
            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(_sagaEntityFail.Id) == null);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsFirstPingFailsToMakeSecondPingAlsoFail()
        {
            var customer = GetCustomer();
            _secondApplication = GetApplicationInArrears(customer); // get application and force it into arrears, this will trigger the ChaseLoanInArrearsSaga
            _secondAppInternalId = ApplicationOperations.GetAppInternalId(_secondApplication);
            customer.AddBadCard(); // add a card with an account number that the mock will deliberately fail, the previous card is expired

            _sagaSecondEntityFail = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == _secondAppInternalId).Single());
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = _sagaSecondEntityFail.Id, State = 0 });
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsFirstPingFailsToMakeSecondPingAlsoFail"), Owner(Owner.JonHurd)]
        public void PaymentsRepaymentRequestSecondPingFailsAfterFirstPingFailed()
        {
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = _sagaSecondEntityFail.Id, State = 1 });

            // Assert there is a request declined record in PaymentCardRepaymentRequests
            var amountDue = _secondApplication.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: _secondAppInternalId,
                                                                                Amount: amountDue,
                                                                                StatusDescription: "Request Declined"
                                                                                 ).Count() == 2);
        }

        [Test, JIRA("UKOPS-419"), DependsOn("PaymentsRepaymentRequestSecondPingFailsAfterFirstPingFailed"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenFirstAndSecondPingFails()
        {
            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(_sagaSecondEntityFail.Id) == null);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenItReceivesATimoutForAPingThatDoesNotExist()
        {
            // Arrange
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Call timeout with a state of 2. The only valid values are 0 and 1 so saga should complete
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 2 });

            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(sagaEntity.Id) == null);
        }

        #region pending test
        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd),Pending("Ticket Not Implemented")]
        public void PaymentsShouldSuppressFiveAmPingIfSuppressionsAreActive()
        {
            // To Do .. not implemented yet
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
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: card, ExpiryDate: expiryDate, DeactivatedOn : null);
        }

        private static void UpdatePrimaryCard(Guid id)
        {
            Drive.Data.Payments.Db.AccountPreferences.UpdateByAccountId(AccountId: id,
                                                                        PrimaryPaymentCardId: _primaryPaymentCardId);
        }

        private static Application GetApplicationInArrears( Customer customer)
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
    }
}
