using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments.Queries
{
	[TestFixture]
	public class GetLoanExtensionPaymentStatusQueryTest
	{
		[Test, AUT(AUT.Uk), JIRA("UK-1323")]
		public void GetLoanExtensionPaymentStatusPaidTest()
		{
			const decimal trustRating = 400.00M;
			var accountId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			var appId = Guid.NewGuid();
			var extensionId = Guid.NewGuid();

			var setupData = new AccountSummarySetupFunctions();

			setupData.Scenario03Setup(appId, paymentCardId, bankAccountId, accountId, trustRating);

			var app = Do.With.Interval(1).Until(() => Drive.Db.Payments.Applications.Single(x => x.ExternalId == appId));
			var fixedTermApp =
				Do.With.Interval(1).Until(
					() => Drive.Db.Payments.FixedTermLoanApplications.Single(x => x.ApplicationId == app.ApplicationId));

			Drive.Api.Commands.Post(new AddPaymentCardCommand
			                        	{
			                        		AccountId = accountId,
			                        		PaymentCardId = paymentCardId,
			                        		CardType = "VISA",
			                        		Number = "4444333322221111",
			                        		HolderName = "Test Holder",
			                        		StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
			                        		ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
			                        		IssueNo = "000",
			                        		SecurityCode = "666",
			                        		IsCreditCard = false,
			                        		IsPrimary = true,
			                        	});

			Do.With.Interval(1).Until(
				() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId && x.AuthorizedOn != null));

			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			                        	{
			                        		ApplicationId = appId,
			                        		ExtendDate = new Date(fixedTermApp.NextDueDate.Value.AddDays(2), DateFormat.Date),
			                        		ExtensionId = extensionId,
			                        		PartPaymentAmount = 20M,
			                        		PaymentCardCv2 = "000",
			                        		PaymentCardId = paymentCardId
			                        	});

			Thread.Sleep(500);
			var resp = Drive.Api.Queries.Post(new GetLoanExtensionPaymentStatusUkQuery { ExtensionId = extensionId });

			Assert.Contains(resp.Values["ExtensionStatus"], "PaymentTaken");
		}

		[Test, AUT(AUT.Uk), JIRA("UK-1323")]
		public void GetLoanExtensionPaymentStatusFailedTest()
		{
			const decimal trustRating = 400.00M;
			var accountId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			var appId = Guid.NewGuid();
			var extensionId = Guid.NewGuid();

			var setupData = new AccountSummarySetupFunctions();

			setupData.Scenario03Setup(appId, paymentCardId, bankAccountId, accountId, trustRating);

			var app = Do.With.Interval(1).Until(() => Drive.Db.Payments.Applications.Single(x => x.ExternalId == appId));
			var fixedTermApp =
				Do.With.Interval(1).Until(
					() => Drive.Db.Payments.FixedTermLoanApplications.Single(x => x.ApplicationId == app.ApplicationId));

			Drive.Api.Commands.Post(new AddPaymentCardCommand
			{
				AccountId = accountId,
				PaymentCardId = paymentCardId,
				CardType = "VISA",
				Number = "4444333322221111",
				HolderName = "Test Holder",
				StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
				ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
				IssueNo = "000",
				SecurityCode = "666",
				IsCreditCard = false,
				IsPrimary = true,
			});

			Do.With.Interval(1).Until(
				() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId && x.AuthorizedOn != null));

			//Using masked FailedCv2 for CardPayment Mock
			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			{
				ApplicationId = appId,
				ExtendDate = new Date(fixedTermApp.NextDueDate.Value.AddDays(2), DateFormat.Date),
				ExtensionId = extensionId,
				PartPaymentAmount = 20M,
				PaymentCardCv2 = "888",
				PaymentCardId = paymentCardId
			});

			Thread.Sleep(500);

			var resp = Do.With.Interval(1).Until(() => Drive.Api.Queries.Post(new GetLoanExtensionPaymentStatusUkQuery { ExtensionId = extensionId }));

			Assert.Contains(resp.Values["ExtensionStatus"], "PaymentFailed");
		}

		[Test, AUT(AUT.Uk), JIRA("UK-1323")]
		public void GetLoanExtensionPaymentStatusPendingTest()
		{
			const decimal trustRating = 400.00M;
			var accountId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			var appId = Guid.NewGuid();
			var extensionId = Guid.NewGuid();

			var setupData = new AccountSummarySetupFunctions();

			setupData.Scenario03Setup(appId, paymentCardId, bankAccountId, accountId, trustRating);

			var app = Do.With.Interval(1).Until(() => Drive.Db.Payments.Applications.Single(x => x.ExternalId == appId));
			var fixedTermApp =
				Do.With.Interval(1).Until(
					() => Drive.Db.Payments.FixedTermLoanApplications.Single(x => x.ApplicationId == app.ApplicationId));

			Drive.Api.Commands.Post(new AddPaymentCardCommand
			{
				AccountId = accountId,
				PaymentCardId = paymentCardId,
				CardType = "VISA",
				Number = "4444333322221111",
				HolderName = "Test Holder",
				StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
				ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
				IssueNo = "000",
				SecurityCode = "666",
				IsCreditCard = false,
				IsPrimary = true,
			});

			Do.With.Interval(1).Until(
				() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId && x.AuthorizedOn != null));

			//Using masked ErrorCv2 for CardPayment Mock
			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			{
				ApplicationId = appId,
				ExtendDate = new Date(fixedTermApp.NextDueDate.Value.AddDays(2), DateFormat.Date),
				ExtensionId = extensionId,
				PartPaymentAmount = 20M,
				PaymentCardCv2 = "999",
				PaymentCardId = paymentCardId
			});

			var resp = Do.With.Interval(1).Until(() => Drive.Api.Queries.Post(new GetLoanExtensionPaymentStatusUkQuery { ExtensionId = extensionId }));

			Assert.Contains(resp.Values["ExtensionStatus"], "Pending");
		}
	}
}
