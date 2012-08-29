using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
	[TestFixture]
	[Parallelizable(TestScope.All)]
	[AUT(AUT.Uk)]
    [Description("Verifies that when an application has been paid off the status history will have PaidInFull as the current status.")]
    public class BankruptcyCollectionSuppressionTests
	{

		private const decimal Amount = 100.00m;
	   
		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-899"), Owner(Owner.AnilKrishnamaneni)]
		public void LiveApplicationBankrupt()
		{
			var caseId = Guid.NewGuid();
			var liveCustomer = CustomerBuilder.New().Build();
			var liveApplication = ApplicationBuilder.New(liveCustomer).Build();
			var liveApplicationInternalID = ApplicationOperations.GetAppInternalId(liveApplication);
			var paymentCard = liveCustomer.GetPaymentCard();
			ApplicationOperations.ReportBankrupt(liveApplication, caseId);
			PaymentOperations.TakePayment(liveApplication.Id, paymentCard, Amount);
			PaymentOperations.CheckPaymentsSupressionTransaction(liveApplicationInternalID, Amount);
		}
		
		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-899"), Owner(Owner.AnilKrishnamaneni)]
		public void DueTodayApplicationBankrupt()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			ApplicationOperations.ReportBankrupt(application, caseId);
			application.MakeDueToday();
			PaymentOperations.CheckPaymentsSupressionTransaction(applicationId, Amount);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-899"), Owner(Owner.AnilKrishnamaneni)]
		public void ArrearApplicationBankrupt()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			var paymentCard = customer.GetPaymentCard();
			application.PutIntoArrears();
			ApplicationOperations.ReportBankrupt(application, caseId);
			PaymentOperations.TakePayment(application.Id, paymentCard, Amount);
			PaymentOperations.CheckPaymentsSupressionTransaction(applicationId, Amount);
		}

		[Test]
		[AUT(AUT.Uk), JIRA("UKOPS-899"), Owner(Owner.AnilKrishnamaneni)]
		public void RAApplicationBankrupt()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var applicationId = ApplicationOperations.GetAppInternalId(application);
			application.CreateRepaymentArrangement();
			ApplicationOperations.ReportBankrupt(application, caseId);
			PaymentOperations.TakePayment(application.Id, customer.GetPaymentCard(), Amount);
			PaymentOperations.CheckPaymentsSupressionTransaction(applicationId, Amount);
		}
	}
}
