using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
	[TestFixture]
	[Parallelizable(TestScope.All)]
	[AUT(AUT.Uk)]
	public class FraudPaymentsCollectionSuppressionTests
	{
		private const decimal Amount = 100.00m;
		private Application _liveApplication;
		private Customer _liveCustomer;
		private int _liveApplicationInternalID;
		private Customer _customer;
		private Application _application;
		private int _applicationId;

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void LiveApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			_liveCustomer = CustomerBuilder.New().Build();
			_liveApplication = ApplicationBuilder.New(_liveCustomer).Build();
			_liveApplicationInternalID = ApplicationOperations.GetAppInternalId(_liveApplication);
			var paymentCard = _liveCustomer.GetPaymentCard();
			ApplicationOperations.SuspectFraud(_liveApplication, _liveCustomer, caseId);
			PaymentOperations.TakePayment(_liveApplication.Id, paymentCard, Amount);
			PaymentOperations.CheckPaymentsSupressionTransaction(_liveApplicationInternalID, Amount);
			ApplicationOperations.ConfirmNotFraud(_liveApplication, _liveCustomer, caseId);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni), DependsOn("LiveApplicationFraud")]
		public void DueTodayPaymentTakenWhenApllicationIsConfirmedNotFraud()
		{
			var amount = _liveApplication.GetDueDateBalance();
			_liveApplication.MakeDueToday();
			PaymentOperations.CheckPaymentTransaction(_liveApplicationInternalID, amount);
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
			PaymentOperations.CheckPaymentsSupressionTransaction(applicationId, Amount);
			ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
			PaymentOperations.TakePayment(application.Id, customer.GetPaymentCard(), Amount);
			PaymentOperations.CheckPaymentTransaction(applicationId, Amount);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void ArrearApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			var paymentCard = customer.GetPaymentCard();
			application.PutIntoArrears();
			ApplicationOperations.SuspectFraud(application, customer, caseId);
			PaymentOperations.TakePayment(application.Id, paymentCard, Amount);
			PaymentOperations.CheckPaymentsSupressionTransaction(applicationId, Amount);
			ApplicationOperations.ConfirmNotFraud(application, customer, caseId);
			PaymentOperations.TakePayment(application.Id, paymentCard, Amount);
			PaymentOperations.CheckPaymentTransaction(applicationId, Amount);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni)]
		public void RAApplicationFraud()
		{
			var caseId = Guid.NewGuid();
			_customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(_customer).Build();
			_applicationId = ApplicationOperations.GetAppInternalId(_application);
			_application.CreateRepaymentArrangement();
			ApplicationOperations.SuspectFraud(_application, _customer, caseId);
			PaymentOperations.TakePayment(_application.Id, _customer.GetPaymentCard(), Amount);
			PaymentOperations.CheckPaymentsSupressionTransaction(_applicationId, Amount);
			ApplicationOperations.ConfirmNotFraud(_application, _customer, caseId);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-885"), Owner(Owner.AnilKrishnamaneni), DependsOn("RAApplicationFraud"), Pending("bug-943")]
		public void RAApplicationcomingfromFraud()
		{
			PaymentOperations.TakePayment(_application.Id, _customer.GetPaymentCard(), Amount);
			PaymentOperations.CheckPaymentTransaction(_applicationId, Amount);
		}
	}
}
