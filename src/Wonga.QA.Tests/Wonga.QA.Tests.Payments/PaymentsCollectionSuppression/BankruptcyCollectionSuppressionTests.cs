using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
	[TestFixture]
	[Parallelizable(TestScope.All)]
	[AUT(AUT.Uk)]
    [Description("Verifies that when an application has been paid off the status history will have PaidInFull as the current status.")]
    [Pending("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
	public class BankruptcyCollectionSuppressionTests
	{
		[Test]
		public void PaymentsCollectionsShouldBeSuppressed_WhenCustomerIsInBankruptcyProvedState()
		{
			var caseId = Guid.NewGuid();
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			var command = new CsReportBankruptcyCommand
			              	{CaseId = caseId, AccountId = customer.Id, ApplicationId = application.Id};

			Drive.Cs.Commands.Post(command);

			dynamic app = null;

			dynamic applicationsTable = Drive.Data.Payments.Db.Applications;
            Do.Until(() => app = applicationsTable.FindBy(ExternalId: application.Id));

			dynamic paymentCollectionSuppressions = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
			Do.Until(() => paymentCollectionSuppressions.FindBy(ApplicationId: app.ApplicationId, 
																BankruptcySuppression: true,
																FraudSuppression: false,
																DCASuppression: false,
																DMPRepaymentArrangementSuppression: false,
																HardshipSuppression: false,
																RepaymentArrangementSuppression: false,
																ComplaintSuppression: false,
																ClearMyBalanceSuppression: false,
																DebtSoldSuppression: false));
		}

		[Test, AUT(AUT.Uk)]
		public void ShouldNotTakeManualPayment_WhenPaymentCollectionsAreSuppressedDueToBankruptcy()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears();

			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			Drive.Cs.Commands.Post(new CsReportBankruptcyCommand
			{
				AccountId = customer.Id,
				ApplicationId = application.Id,
				CaseId = Guid.NewGuid()
			});

			dynamic app = null;
			dynamic applicationsTable = Drive.Data.Payments.Db.Applications;
			Do.Until(() => app = applicationsTable.FindBy(ExternalId: application.Id));

			dynamic paymentsSuppressionsTable = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
			Do.Until(() => paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId, BankruptcySuppression: true));

			Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = 100.00m,
				ApplicationId = application.Id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = customer.GetPaymentCard(),
				PaymentId = Guid.NewGuid(),
				SalesforceUser = "test.user@wonga.com"
			});

			var paymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;

			Do.Until(() => paymentRequests.FindAll(paymentRequests.ApplicationId == applicationId &&
												   paymentRequests.StatusDescription == "PaymentCollectionsSuppressed"));
		}
	}
}
