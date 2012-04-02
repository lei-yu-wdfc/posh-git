using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture, Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
	public class SalesforcePushGuarantorDetailsTests : SalesforceTestBase
    {
        private const int NrOfGuarantors = 3;

        [Test, AUT(AUT.Wb), JIRA("SME-375"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void ShouldUpdateSalesforceContact_WhenIndividualGuarantorSignsApplicationTerms()
		{
			const int expectedStatus = 104;

			var mainApplicant = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NrOfGuarantors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }

			var organisation = OrganisationBuilder.New(mainApplicant).Build();
            var applicationBuilder = ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder;
            applicationBuilder.WithGuarantors(listOfGuarantors).Build();

			Do.Until(() => Salesforce.GetContactByStatus(mainApplicant.Id, expectedStatus));
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		public void ShouldUpdateSalesforceContact_WhenMainApplicantIsAccepted()
		{
			const int expectedStatus = 105;

			var mainApplicant = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NrOfGuarantors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }
			var organisation = OrganisationBuilder.New(mainApplicant).Build();
            var applicationBuilder = ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder;
            applicationBuilder.WithGuarantors(listOfGuarantors).Build();

			Do.Until(() => Salesforce.GetContactByStatus(mainApplicant.Id, expectedStatus));
		}
	}
}
