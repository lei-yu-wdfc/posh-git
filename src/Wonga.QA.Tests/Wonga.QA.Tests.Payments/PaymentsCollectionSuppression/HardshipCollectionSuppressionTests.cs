using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
	[TestFixture]
	[Parallelizable(TestScope.All)]
	[AUT(AUT.Uk)]
	[Pending("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
	public class HardshipCollectionSuppressionTests
	{
		[Test, AUT(AUT.Uk)]
		public void ShouldNotTakeManualPayment_WhenPaymentCollectionsAreSuppressedDueToHardship()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			Drive.Cs.Commands.Post(new CsReportHardshipCommand
			{
				AccountId = customer.Id,
				ApplicationId = application.Id,
				CaseId = Guid.NewGuid()
			});

			dynamic app = null;
			dynamic applicationsTable = Drive.Data.Payments.Db.Applications;
			Do.Until(() => app = applicationsTable.FindBy(ExternalId: application.Id));

			dynamic paymentsSuppressionsTable = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
			Do.Until(() => paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId, HardshipSuppression: true));

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

		[Test, AUT(AUT.Uk)]
		public void ShouldSuspendInterestAndServiceFeeAccrual_WhenApplicationIsInHardshipStatus()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			Drive.Cs.Commands.Post(new CsReportHardshipCommand
			{
				AccountId = customer.Id,
				ApplicationId = application.Id,
				CaseId = Guid.NewGuid()
			});

			dynamic applicationsTable = Drive.Data.Payments.Db.Applications;
			Do.Until(() => applicationsTable.FindBy(ExternalId: application.Id));

			var transactions = Drive.Data.Payments.Db.Transactions;

			Do.Until(() => transactions.FindAll(transactions.ApplicationEntity.ExternalId == application.Id
											&& transactions.PostedOn != null
											&& transactions.Scope == (int)PaymentTransactionScopeEnum.Other
											&& transactions.Type == PaymentTransactionEnum.SuspendInterestAccrual.ToString()));

			Do.Until(() => transactions.FindAll(transactions.ApplicationEntity.ExternalId == application.Id
											&& transactions.PostedOn != null
											&& transactions.Scope == (int)PaymentTransactionScopeEnum.Other
											&& transactions.Type == PaymentTransactionEnum.SuspendServiceFee.ToString()));
		}
	}
}
