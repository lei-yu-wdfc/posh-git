using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointTooManyLoansAtAddressTest
	{
		private const RiskMask TestMask = RiskMask.TESTTooManyLoansAtAddress;

        [Test, AUT(AUT.Uk), JIRA("UK-848")]
        public void AcceptForOneApplication()
        {
            const string foreName = "Janet";
			const string surName = "Bernadette";
           
            var customerBuilder = CustomerBuilder.New().WithEmployer(TestMask).WithForename(foreName).WithSurname(surName);
            customerBuilder.ScrubForename(foreName);
            customerBuilder.ScrubSurname(surName);

            var customer = customerBuilder.Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.TooManyLoansAtAddress));
        }

		[Test, AUT(AUT.Uk), JIRA("UK-848"), Category(TestCategories.CoreTest)]
        public void DeclinedIfTooManyOpenLoans()
        {
            /* The test scenario is that there cant be more then 3 loans at the same address
             * 1 - Create 3 applications at the same exact address
             * 2 - Sign them 
             * 3- Try to create a 4th one */

            var flat = Get.RandomInt(1, 10000).ToString();
            var houseNumber = Get.RandomInt(1, 10000).ToString();
            var houseName = Get.RandomString(7);
            var street = Get.RandomString(7);
            var district = Get.RandomString(7);
            const string town = "Runcorn";
            const string county = "Cheshire";
            const string postcode = "WA7 4JJ";

            var customer1 = CustomerBuilder.New().WithEmployer(TestMask).WithFlatInAddress(flat).
                WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).
                WithPostcodeInAddress(postcode).Build();
            var application1 = ApplicationBuilder.New(customer1).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var customer2 = CustomerBuilder.New().WithEmployer(TestMask).WithFlatInAddress(flat).
                WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).
                WithPostcodeInAddress(postcode).Build();
            var application2 = ApplicationBuilder.New(customer2).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var customer3 = CustomerBuilder.New().WithEmployer(TestMask).WithFlatInAddress(flat).
                WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).
                WithPostcodeInAddress(postcode).Build();

            //lets try to create the 4th one and see the failure
            var application3 = ApplicationBuilder.New(customer3).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application3.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(
                Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId,
                                                                             RiskCheckpointStatus.Failed),
                Get.EnumToString(RiskCheckpointDefinitionEnum.TooManyLoansAtAddress));
        }
	}
}
