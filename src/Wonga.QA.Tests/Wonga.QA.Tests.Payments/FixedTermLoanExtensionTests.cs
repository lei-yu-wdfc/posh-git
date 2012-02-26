using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
	public class FixedTermLoanExtensionTests
	{
		[Test, AUT(AUT.Uk), JIRA("UK-598"), 
			Description("Check that customer can extend, when 7 days (or more) left to repayment date"),
			Description("Check that customer can't extend when less than 7 days left to reapyment date"),
			Description("Check that customer can't extend to earlier date than his reapayment date")
		]
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
				ExtendDate = DateTime.Today.AddDays(extendTerm + loanTerm).Date
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

		[Test, AUT(AUT.Uk), JIRA("UK-598"), Description("Check that extend is not done when invalid data used")]
		public void LoanNotExtendsWithWrongData(int loanTerm, int extendTerm, bool available)
		{



		}


		[FixtureTearDown]
		public void TearDownOverride()
		{
			ConfigurationFunctions.ResetQaUtcNowOverride();
		}


	}
}