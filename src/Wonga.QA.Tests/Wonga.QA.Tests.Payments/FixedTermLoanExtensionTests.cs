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
	[TestFixture]
	public class FixedTermLoanExtensionTests
	{
		[Test, AUT(AUT.Uk), JIRA("UK-598"), Ignore("API tests should be unignored when Extend Functionality will be implemented"),
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
				.WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted)
				.WithLoanAmount(100)
				.WithLoanTerm(loanTerm)
				.Build();

			var ftApp = Driver.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == app.Id);
			Assert.IsNotNull(ftApp);

			ConfigurationFunctions.SetupQaUtcNowOverride((ftApp.NextDueDate ?? ftApp.PromiseDate).AddDays(-daysToDueDate));
			//Check Extension available.
			ApiResponse parm = Driver.Api.Queries.Post(new GetFixedTermLoanExtensionParametersQuery
			{
				AccountId = cust.Id,
			});

			var dueDate = (ftApp.NextDueDate ?? ftApp.PromiseDate);
			Assert.GreaterThanOrEqualTo(available ? 0 : 1, parm.GetErrors().Length);
			Assert.AreEqual(available, bool.Parse(parm.Values["IsExtendEnabled"].Single()));
			Assert.GreaterThan(DateTime.Parse(parm.Values["MinExtendDate"].Single()), dueDate);

			//Check calculations for extend
			ApiResponse calc = Driver.Api.Queries.Post(new GetFixedTermLoanExtensionCalculationQuery
			{
				ApplicationId = app.Id,
				ExtendDate = new Date(DateTime.Today.AddDays(extendTerm + loanTerm), DateFormat.Date)
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
		[Row(true, true, false, true, Order = 0)]//has valid loan data
		[Row(false, false, false, false, Order = 1)]//no application exists
		[Row(true, false, false, false, Order = 2)]//no active loan
		[Row(true, true, true, false, Order = 3)]//in arrears
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

				ftApp = Driver.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == app.Id);

				if (inArrears)
					app.PutApplicationIntoArrears();
			}


			var cardId = (appExists ? cust.GetPaymentCard() : Guid.NewGuid());
			var extendDate = new Date(appExists
			                 	? (ftApp.NextDueDate ?? ftApp.PromiseDate).AddDays(3)
			                 	: Data.RandomDate(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1)), DateFormat.Date);

			ConfigurationFunctions.SetupQaUtcNowOverride((ftApp.NextDueDate ?? ftApp.PromiseDate).AddDays(-3));

			var cmd = new CreateFixedTermLoanExtensionCommand
			          	{
			          		ApplicationId = appExists ? app.Id : Guid.NewGuid(),
			          		ExtendDate = extendDate,
			          		ExtensionId = Guid.NewGuid(),
			          		PaymentCardCv2 = appExists ? "123" : null,
			          		PaymentCardId = cardId
			          	};

			var cmdAct = new Gallio.Common.Action(() => Driver.Api.Commands.Post(cmd));
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


	}
}