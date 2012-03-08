using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
	public class GetBusinessFixedInstallmentLoanCalculationTests
	{
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void GetBusinessFixedInstallmentLoanCalculation()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var response = Driver.Api.Queries.Post(new GetBusinessFixedInstallmentLoanCalculationWbUkQuery
				{ LoanAmount = 5000, Term = 16, ApplicationId = application.Id});

			Assert.IsNotNull(response);
			Assert.AreEqual("10000.00", response.Values["LoanAmount"].SingleOrDefault(), "Expected LoanAmount value is incorrect.");
			Assert.AreEqual("16", response.Values["Term"].SingleOrDefault(), "Expected Term value is incorrect.");
			Assert.AreEqual("5.00", response.Values["ApplicationFeeRate"].SingleOrDefault(), "Expected ApplicationFeeRate value is incorrect.");
			Assert.AreEqual("4800.00", response.Values["InterestAmount"].SingleOrDefault(), "Expected InterestAmount value is incorrect.");
			Assert.AreEqual("3.000000", response.Values["WeeklyInterestRate"].SingleOrDefault(), "Expected WeeklyInterestRate value is incorrect.");
			Assert.AreEqual("15300.00", response.Values["TotalRepayable"].SingleOrDefault(), "Expected TotalRepayable value is incorrect.");
			Assert.AreEqual("956.25", response.Values["WeeklyRepayable"].SingleOrDefault(), "Expected WeeklyRepayable value is incorrect.");
		}


		/// <summary>
		/// Scenario - Set the term to minimal allowed
		/// </summary>
		/// <scenario>
		/// Given the customer on the page with the sliders displaying the interest rate based on the risk tier
		/// When the customer sets the term to below minimal allowed
		/// Then return minimal term
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void GetBusinessFixedInstallmentLoanCalculation_SetTheTermToMinimalAllowed()
		{
			var offerResponse = Driver.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());
			int minTerm;

			int.TryParse(offerResponse.Values["TermMin"].SingleOrDefault(), out minTerm);

			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var response = Driver.Api.Queries.Post(new GetBusinessFixedInstallmentLoanCalculationWbUkQuery 
				{ LoanAmount = 5000, Term = minTerm - 1, ApplicationId = application.Id });

			Assert.IsNotNull(response);
			Assert.AreEqual(offerResponse.Values["TermMin"].SingleOrDefault(), response.Values["Term"].SingleOrDefault());
		}

		/// <summary>
		/// Scenario - Set the term to max allowed
		/// </summary>
		/// <scenario>
		/// Given the customer on the page with the sliders displaying the interest rate based on the risk tier
		/// When the customer sets the term to more than the maximum allowed
		/// Then return maximum term
		/// </scenario>
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void GetBusinessFixedInstallmentLoanCalculation_SetTheTermToMaximumAllowed()
		{
			var offerResponse = Driver.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());
			int maxTerm;

			int.TryParse(offerResponse.Values["TermMax"].SingleOrDefault(), out maxTerm);

			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var response = Driver.Api.Queries.Post(new GetBusinessFixedInstallmentLoanCalculationWbUkQuery { LoanAmount = 5000, Term = maxTerm + 1, ApplicationId = application.Id });

			Assert.IsNotNull(response);
			Assert.AreEqual(offerResponse.Values["TermMax"].SingleOrDefault(), response.Values["Term"].SingleOrDefault());
		}
	}
}
