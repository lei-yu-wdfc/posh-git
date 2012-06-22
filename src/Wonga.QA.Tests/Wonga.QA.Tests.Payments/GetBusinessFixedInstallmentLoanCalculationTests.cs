using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
    [Parallelizable(TestScope.All)]
	public class GetBusinessFixedInstallmentLoanCalculationTests
	{
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void GetBusinessFixedInstallmentLoanCalculation()
		{
            var customer = CustomerBuilder.New().WithGender(GenderEnum.Female).WithSpecificAge(40).WithNumberOfDependants(0).Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id));

			var response = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanCalculationWbUkQuery
				{ LoanAmount = 5000, Term = 16, ApplicationId = application.Id});

			Assert.IsNotNull(response);
			Assert.AreEqual(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.LoanAmount.ToString(), response.Values["LoanAmount"].SingleOrDefault(), "Expected LoanAmount value is incorrect.");
			Assert.AreEqual("16", response.Values["Term"].SingleOrDefault(), "Expected Term value is incorrect.");
			Assert.AreEqual(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.ApplicationFee.ToString(), response.Values["ApplicationFeeRate"].SingleOrDefault(), "Expected ApplicationFeeRate value is incorrect.");
			Assert.AreEqual(applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.InterestRate.ToString(), response.Values["WeeklyInterestRate"].SingleOrDefault(), "Expected WeeklyInterestRate value is incorrect.");
			Assert.AreEqual("3200.00", response.Values["InterestAmount"].SingleOrDefault(), "Expected InterestAmount value is incorrect.");
			Assert.AreEqual("13700.00", response.Values["TotalRepayable"].SingleOrDefault(), "Expected TotalRepayable value is incorrect.");
			Assert.AreEqual("856.25", response.Values["WeeklyRepayable"].SingleOrDefault(), "Expected WeeklyRepayable value is incorrect.");
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
			var offerResponse = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());
			int minTerm;

			int.TryParse(offerResponse.Values["TermMin"].SingleOrDefault(), out minTerm);

			var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var response = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanCalculationWbUkQuery 
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
			var offerResponse = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());
			int maxTerm;

			int.TryParse(offerResponse.Values["TermMax"].SingleOrDefault(), out maxTerm);

			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var response = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanCalculationWbUkQuery { LoanAmount = 5000, Term = maxTerm + 1, ApplicationId = application.Id });

			Assert.IsNotNull(response);
			Assert.AreEqual(offerResponse.Values["TermMax"].SingleOrDefault(), response.Values["Term"].SingleOrDefault());
		}
	}
}
