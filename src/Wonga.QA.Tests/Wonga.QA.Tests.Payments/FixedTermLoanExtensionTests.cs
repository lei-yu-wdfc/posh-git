using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class FixedTermLoanExtensionTests
	{
        const string ValidCardNumber = "4444333322221111";
        const string Cv2MaskFailedPayment = "888";
        const string Cv2MaskPaymentSucceeds = "123";

        const string VisaCardType = "Visa";
        const string ValidIssueNumber = "000";

        [Test, AUT(AUT.Uk), JIRA("UK-598"),
         Ignore("API tests should be unignored when Extend Functionality will be implemented"),
         Description("Check that customer can extend, when 7 days (or more) left to repayment date"),
         Description("Check that customer can't extend when less than 7 days left to reapyment date"),
         Description("Check that customer can't extend to earlier date than his reapayment date")]
        [Row(10, 5, true, 8, Order = 0)]
        [Row(10, 10, true, 8, Order = 1)]
        [Row(10, 50, false, 8, Order = 2)]
        [Row(10, 10, false, 6, Order = 3)]
		public void LoanExtendsInValidTimeFrame(int loanTerm, int extendTerm, bool available, int daysToDueDate)
		{
			//create customer with live application.
			Customer cust = CustomerBuilder.New().Build();
			Do.Until(cust.GetBankAccount);
			Do.Until(cust.GetPaymentCard);
			Application app = ApplicationBuilder.New(cust)
				.WithExpectedDecision(ApplicationDecisionStatus.Accepted)
				.WithLoanAmount(100)
				.WithLoanTerm(loanTerm)
				.Build();

			var ftApp = Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == app.Id);
			Assert.IsNotNull(ftApp);

			ConfigurationFunctions.SetupQaUtcNowOverride((ftApp.NextDueDate ?? ftApp.PromiseDate).AddDays(-daysToDueDate));
			//Check Extension available.
			ApiResponse parm = Drive.Api.Queries.Post(new GetFixedTermLoanExtensionParametersQuery
			                                          	{
			                                          		AccountId = cust.Id,
			                                          	});

			var dueDate = (ftApp.NextDueDate ?? ftApp.PromiseDate);
			Assert.GreaterThanOrEqualTo(available ? 0 : 1, parm.GetErrors().Length);
			Assert.AreEqual(available, bool.Parse(parm.Values["IsExtendEnabled"].Single()));
			Assert.GreaterThan(DateTime.Parse(parm.Values["MinExtendDate"].Single()), dueDate);

			//Check calculations for extend
			ApiResponse calc = Drive.Api.Queries.Post(new GetFixedTermLoanExtensionCalculationQuery
			                                          	{
			                                          		ApplicationId = app.Id,
			                                          		ExtendDate =
			                                          			new Date(DateTime.Today.AddDays(extendTerm + loanTerm), DateFormat.Date)
			                                          	});

			if (available)
			{
				Assert.IsEmpty(calc.Values["ErrorMessage"]);

			}
			else
			{
				Assert.IsNotEmpty(calc.Values["ErrorMessage"]);
			}



		}

		[Test, AUT(AUT.Uk), JIRA("UK-598"),
		 Description("Check that extend is not done when invalid data used"),
		 Ignore("API tests should be unignored when Extend Functionality will be implemented")]
		[Row(true, true, false, true, Order = 0)] //has valid loan data
		[Row(false, false, false, false, Order = 1)] //no application exists
		[Row(true, false, false, false, Order = 2)] //no active loan
		[Row(true, true, true, false, Order = 3)] //in arrears
		public void LoanNotExtendsWithWrongData(bool appExists, bool hasActiveLoan, bool inArrears, bool shouldPass)
		{
			//create customer
			Customer cust = CustomerBuilder.New().Build();
			Do.Until(cust.GetBankAccount);
			Do.Until(cust.GetPaymentCard);

			//create application if needed
			Application app = null;
			FixedTermLoanApplicationEntity ftApp = null;
			if (appExists)
			{
				app = ApplicationBuilder.New(cust)
					.WithLoanAmount(100)
					.Build();

				ftApp = Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == app.Id);

				if (inArrears)
					app.PutApplicationIntoArrears();
			}


			var cardId = (appExists ? cust.GetPaymentCard() : Guid.NewGuid());
			var extendDate = new Date(appExists
			                          	? (ftApp.NextDueDate ?? ftApp.PromiseDate).AddDays(3)
			                          	: Get.RandomDate(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1)), DateFormat.Date);

			ConfigurationFunctions.SetupQaUtcNowOverride((ftApp.NextDueDate ?? ftApp.PromiseDate).AddDays(-3));

			var cmd = new CreateFixedTermLoanExtensionCommand
			          	{
			          		ApplicationId = appExists ? app.Id : Guid.NewGuid(),
			          		ExtendDate = extendDate,
			          		ExtensionId = Guid.NewGuid(),
			          		PaymentCardCv2 = appExists ? "123" : null,
			          		PaymentCardId = cardId
			          	};

			var cmdAct = new Gallio.Common.Action(() => Drive.Api.Commands.Post(cmd));
			if (shouldPass)
				Assert.DoesNotThrow(cmdAct);
			else
				Assert.Throws<Exception>(cmdAct);

		}

		[FixtureTearDown]
		public void TearDownOverride()
		{
			ConfigurationFunctions.ResetQaUtcNowOverride();
		}

		[Test, AUT(AUT.Uk), JIRA("UK-427")]
		public void CreateFixedTermLoanExtensionTest()
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
			                        		CardType = VisaCardType,
                                            Number = ValidCardNumber,
			                        		HolderName = "Test Holder",
			                        		StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
			                        		ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
			                        		IssueNo = ValidIssueNumber,
                                            SecurityCode = Cv2MaskPaymentSucceeds,
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
                                            PaymentCardCv2 = Cv2MaskPaymentSucceeds,
			                        		PaymentCardId = paymentCardId
			                        	});

			var loanExtension =
				Do.With.Interval(1).Until(
					() =>
					Drive.Db.Payments.LoanExtensions.Single(x => x.ExternalId == extensionId && x.ApplicationId == app.ApplicationId && (x.PartPaymentTakenOn != null || x.PartPaymentFailedOn != null)));


			Assert.IsNotNull(loanExtension, "A loan extension should be created");
			Assert.IsNotNull(loanExtension.PartPaymentTakenOn, "Loan extension payment should be taken");
		}

		[Test, AUT(AUT.Uk), JIRA("UK-427")]
		public void CreateFixedTermLoanExtensionFailedPaymentTest()
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
			                        		AccountId = app.AccountId,
			                        		PaymentCardId = app.PaymentCardGuid,
			                        		CardType = VisaCardType,
                                            Number = ValidCardNumber,
			                        		HolderName = "Test Holder",
			                        		StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
			                        		ExpiryDate = DateTime.Today.AddMonths(+1).ToDate(DateFormat.YearMonth),
			                        		IssueNo = ValidIssueNumber,
			                        		SecurityCode = Cv2MaskFailedPayment,
			                        		IsCreditCard = false,
			                        		IsPrimary = true,
			                        	});

			Do.With.Interval(1).Until(() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId));

			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			                        	{
			                        		ApplicationId = appId,
			                        		ExtendDate = new Date(fixedTermApp.NextDueDate.Value.AddDays(2), DateFormat.Date),
			                        		ExtensionId = extensionId,
			                        		PartPaymentAmount = 20M,
                                            PaymentCardCv2 = Cv2MaskFailedPayment,
			                        		PaymentCardId = paymentCardId
			                        	});

			var loanExtension =
				Do.With.Interval(1).Until(
					() =>
                    Drive.Db.Payments.LoanExtensions.Single(x => x.ExternalId == extensionId && x.ApplicationId == app.ApplicationId && (x.PartPaymentTakenOn != null || x.PartPaymentFailedOn != null)));


			Assert.IsNotNull(loanExtension, "A loan extension should be created");
			Assert.IsNull(loanExtension.PartPaymentTakenOn,
			              "Loan extension payment should not be taken, PartPaymentTakenOn should be null");
			Assert.IsNotNull(loanExtension.PartPaymentFailedOn, "Loan extension payment should not be taken");
		}

		[Test, AUT(AUT.Uk), JIRA("Uk-971")]
		public void SignFixedTermLoanExtensionTest()
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
			                        		CardType = VisaCardType,
			                        		Number = ValidCardNumber,
			                        		HolderName = "Test Holder",
			                        		StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
			                        		ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
			                        		IssueNo = ValidIssueNumber,
			                        		SecurityCode = Cv2MaskPaymentSucceeds,
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
                                            PaymentCardCv2 = Cv2MaskPaymentSucceeds,
			                        		PaymentCardId = paymentCardId
			                        	});

			var loanExtension =
                Do.With.Interval(1).Until(() => Drive.Db.Payments.LoanExtensions.Single(x => x.ExternalId == extensionId && x.ApplicationId == app.ApplicationId && (x.PartPaymentTakenOn != null || x.PartPaymentFailedOn != null)));

			Drive.Api.Commands.Post(new SignFixedTermLoanExtensionCommand
			                        	{
			                        		ApplicationId = appId,
			                        		ExtensionId = extensionId
			                        	});

			loanExtension.Refresh();

            Do.With.Interval(1).Until(() => Drive.Db.Payments.LoanExtensions.Single(x => x.ExternalId == extensionId && x.ApplicationId == app.ApplicationId && x.SignedOn != null ));

            Assert.IsNotNull(loanExtension.SignedOn, "Loan Extension should be signed");
		}
	}
}
 