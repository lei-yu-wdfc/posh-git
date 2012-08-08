using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Test, JIRA("UKOPS-419"),Owner(Owner.JonHurd)]
        public void PaymentsShouldCreateNewTransactionWhenFiveAmCollectionSucceeds()
        {
            // Arrange
            var customer = GetCustomer();            
            var application = GetApplicationInArrears( customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));            
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            
            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Act
            // Call timeout with a state of 0 representing the first ping at 5 am ping
            Drive.Msmq.Payments.Send(message: new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 0 });

            // Assert there should be a credit transaction in the table representing the payment
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appInternalId,
                                                                                Type: PaymentTransactionEnum.CardPayment.ToString(),
                                                                                Amount: amountDue * -1,
                                                                                Scope: (int)PaymentTransactionScopeEnum.Credit,
                                                                                Reference: "Automatic Ping (Card)").Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenFiveAmCollectionSucceeds()
        {
            // Arrange
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Act
            // Call timeout with a state of 0 representing the first ping at 5 am ping
            Drive.Msmq.Payments.Send( new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 0 });

            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(sagaEntity.Id) == null);


        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldCreateNewTransactionWhenEightAmCollectionSucceeds()
        {
            // Arrange
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Act
            // Call timeout with a state of 1 representing the second ping at 8 am ping
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 1 });

            // Assert there should be a credit transaction in the table representing the payment
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appInternalId,
                                                                                Type: PaymentTransactionEnum.CardPayment.ToString(),
                                                                                Amount: amountDue * -1,
                                                                                Scope: (int)PaymentTransactionScopeEnum.Credit,
                                                                                Reference: "Automatic Ping (Card)").Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldMarkSagaAsCompleteWhenEightAmCollectionSucceeds()
        {
            // Arrange
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer);
            SetCardExpiryDate(customer.GetPaymentCard(), DateTime.Now.AddYears(1));
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Act
            // Call timeout with a state of 1 representing the second ping at 8 am ping
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 1 });

            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(sagaEntity.Id) == null);


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

            // Act
            // Call timeout with a state of 2. The only valid values are 0 and 1 so saga should complete
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 2 });

            // Assert that saga is complete
            Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindById(sagaEntity.Id) == null);


        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFirstPingFails()
        {
            // Arrange
            dynamic ChaseLoanInArrearsSagaEntities = Drive.Data.OpsSagas.Db.ChaseLoanInArrearsSagaEntity;
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer); // get application and force it into arrears, this will trigger the ChaseLoanInArrearsSaga
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            customer.AddBadCard(); // add a card with an account number that the mock will deliberately fail, the previous card is expired

            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Act
            // Call timeout with a state of 0 representing the first ping at 5 am ping
            Drive.Msmq.Payments.Send( new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 0 });

            // Assert there is a request declined record in PaymentCardRepaymentRequests
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: appInternalId,
                                                                                Amount: amountDue ,                                                                                
                                                                                StatusDescription:"Request Declined"
                                                                                 ).Count() == 1);
        }

        [Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        public void PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenSecondPingFails()
        {
            // Arrange
            var customer = GetCustomer();
            var application = GetApplicationInArrears(customer); // get application and force it into arrears, this will trigger the ChaseLoanInArrearsSaga
            var appInternalId = ApplicationOperations.GetAppInternalId(application);
            customer.AddBadCard(); // get a card with an account number that the mock will deliberately fail, the previous card is expired
            var sagaEntity = Do.With.Timeout(2).Until(() => ChaseLoanInArrearsSagaEntities.FindAll(ChaseLoanInArrearsSagaEntities.ApplicationId == appInternalId).Single());

            // Act
            // Call timeout with a state of 0 representing the first ping at 5 am ping
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { Expires = DateTime.UtcNow, SagaId = sagaEntity.Id, State = 1 });

            // Assert there is a request declined record in PaymentCardRepaymentRequests
            var amountDue = application.GetDueDateBalance();
            Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: appInternalId,
                                                                                Amount: amountDue,
                                                                                StatusDescription: "Request Declined"
                                                                                 ).Count() == 1);
        }

        //[Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        //public void PaymentsShouldSuppressFiveAmPingIfSuppressionsAreActive()
        //{
        //    // To Do .. not implemented yet
        //}

        //[Test, JIRA("UKOPS-419"), Owner(Owner.JonHurd)]
        //public void PaymentsShouldSuppressEightAmPingIfSuppressionsAreActive()
        //{
        //    // To Do .. not implemented yet
        //}

        private static void SetCardExpiryDate(Guid card, DateTime expiryDate)
        {
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: card, ExpiryDate: expiryDate, DeactivatedOn : null);
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
