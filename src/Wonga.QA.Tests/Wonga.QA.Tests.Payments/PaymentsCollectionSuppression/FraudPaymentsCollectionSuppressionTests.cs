using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
    [TestFixture]
    [Parallelizable(TestScope.All), Pending("Depends on salesforce tc so will not run reliably on RC")]
    [AUT(AUT.Uk)]
    public class FraudPaymentsCollectionSuppressionTests
    {
        private static readonly dynamic PaymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;
		private static readonly dynamic PaymentTransactions = Drive.Data.Payments.Db.Transactions;
    	private const decimal Amount = 100.00m;
    	private Application _liveApplication;
    	private Customer _liveCustomer;
    	private int _liveApplicationInternalID;

        [Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void LiveApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			_liveCustomer = CustomerBuilder.New().Build();
			_liveApplication = ApplicationBuilder.New(_liveCustomer).Build();
			_liveApplicationInternalID = ApplicationOperations.GetAppInternalId(_liveApplication);
			ApplicationOperations.SuspectFraud(_liveApplication, _liveCustomer, caseId);
			TakePayment(_liveApplication.Id, _liveCustomer.GetPaymentCard());
			CheckPaymentsSupressionTransaction(_liveApplicationInternalID);
			ApplicationOperations.ConfirmNotFraud(_liveApplication, _liveCustomer, caseId);
        }

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni), DependsOn("LiveApplicationFraud")]
		public void DueTodayPaymentTakenWhenApllicationIsConfirmedNotFraud()
		{
			_liveApplication.MakeDueToday();
			CheckPaymentTransaction(_liveApplicationInternalID, 106.7m);
		}

    	[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void DueTodayApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			ApplicationOperations.SuspectFraud(application, customer, caseId);
			application.MakeDueToday();
			CheckPaymentsSupressionTransaction(applicationId);
			ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
			TakePayment(application.Id, customer.GetPaymentCard());
	    }

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void ArrearApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			application.PutIntoArrears();
			ApplicationOperations.SuspectFraud(application, customer, caseId);
			TakePayment(application.Id, customer.GetPaymentCard());
			CheckPaymentsSupressionTransaction(applicationId);
			ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
			TakePayment(application.Id, customer.GetPaymentCard());
			CheckPaymentTransaction(applicationId, Amount);
		}
		
		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void RAApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			application.CreateRepaymentArrangement();
			ApplicationOperations.SuspectFraud(application, customer, caseId);
			TakePayment(application.Id, customer.GetPaymentCard());
			CheckPaymentsSupressionTransaction(applicationId);
			ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
			TakePayment(application.Id, customer.GetPaymentCard());
			CheckPaymentTransaction(applicationId, Amount);
		}

		#region Helpers

    	private static void TakePayment(Guid id, Guid paymentCard)
    	{
    		Drive.Cs.Commands.Post(new TakePaymentManualCommand
    		                       	{
    		                       		Amount = Amount,
    		                       		ApplicationId = id,
    		                       		Currency = CurrencyCodeEnum.GBP,
    		                       		PaymentCardId = paymentCard,
    		                       		PaymentId = Guid.NewGuid(),
    		                       		SalesforceUser = "test.user@wonga.com"
    		                       	});
    	}
		
		private static void CheckPaymentTransaction(int applicationID,decimal paymentamount)
		{
			paymentamount = paymentamount*-1;
			Do.Until(() => PaymentTransactions.FindBy(ApplicationId:applicationID , Amount:paymentamount).Single());
		}

    	private static void CheckPaymentsSupressionTransaction(int applicationId)
    	{
    		Do.Until(() => PaymentRequests.FindAll(PaymentRequests.ApplicationId == applicationId &&
    		                                       PaymentRequests.StatusDescription == "PaymentCollectionsSuppressed" ).Single());
    	}
		#endregion
    }
}
