using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
    [Parallelizable(TestScope.All)]
	public class UpdateLoanTermTests
	{
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void ShouldUpdateApplicationWithNewLoanTerm()
		{
			int newLoanTerm = 5;

			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();
			
			ApplicationEntity applicationEntity = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id));
			var currentTerm = applicationEntity.BusinessFixedInstallmentLoanApplicationEntity.Term;

			if (currentTerm == newLoanTerm) newLoanTerm += 1;

			Drive.Api.Commands.Post(new UpdateLoanTermWbUkCommand { ApplicationId = application.Id, Term = newLoanTerm });

			Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id
				&& a.BusinessFixedInstallmentLoanApplicationEntity.Term == newLoanTerm));
		}

		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void ShouldAdjustLoanTermBeforeUpdatingApplication_WhenLoanTermIsLessThanMinTermAllowed()
		{
			var offerResponse = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());
			int minTerm;
			int.TryParse(offerResponse.Values["TermMin"].SingleOrDefault(), out minTerm);

			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var exception = Assert.Throws<ValidatorException>(
				() => Drive.Api.Commands.Post(new UpdateLoanTermWbUkCommand {ApplicationId = application.Id, Term = minTerm - 1}));

			Assert.Contains(exception.Message, "Payments_LoanTerm_Invalid");
		}


		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void ShouldAdjustLoanTermBeforeUpdatingApplication_WhenLoanTermIsMoreThanMaxTermAllowed()
		{
			var offerResponse = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());
			int maxTerm;
			int.TryParse(offerResponse.Values["TermMax"].SingleOrDefault(), out maxTerm);

			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var exception = Assert.Throws<ValidatorException>(
				() => Drive.Api.Commands.Post(new UpdateLoanTermWbUkCommand { ApplicationId = application.Id, Term = maxTerm + 1 }));

			Assert.Contains(exception.Message, "Payments_Term_GreaterTermMax");
		}
	}
}
