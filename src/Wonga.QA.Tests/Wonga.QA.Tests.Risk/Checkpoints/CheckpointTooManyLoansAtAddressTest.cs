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

        [Test, AUT(AUT.Uk), JIRA("UK-913")]
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

        [Test, AUT(AUT.Uk), JIRA("UK-913")]
        public void DeclinedIfTooManyOpenLoans()
        {
            /* The test scenario is that there cant be more then 3 loans at the same address
             * 1 - Create 3 applications at the same exact address
             * 2 - Sign them 
             * 3- Try to create a 4th one */
            
            const string flat = "1";
            const string houseNumber = "14";
            const string houseName = "HouseName";
            const string street = "Mather Avenue";
            const string district = "Weston Point";
            const string town = "Runcorn";
            const string county = "Cheshire";
            const string postcode = "WA7 4JJ";

            var customer1 = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).WithFlatInAddress(flat).
                    WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                    WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).WithPostcodeInAddress(postcode).Build();
            var application1 = ApplicationBuilder.New(customer1).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var customer2 = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).WithFlatInAddress(flat).
                    WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                    WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).WithPostcodeInAddress(postcode).Build();
            var application2 = ApplicationBuilder.New(customer2).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var customer3 = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).WithFlatInAddress(flat).
                   WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                   WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).WithPostcodeInAddress(postcode).Build();
            var application3 = ApplicationBuilder.New(customer3).Build();
            
            //I have 2 apps - lets sign them 
            var riskDb = Drive.Db.Risk;
            var riskapp1 = riskDb.RiskApplications.Single(x => x.ApplicationId == application1.Id);
            var riskapp2 = riskDb.RiskApplications.Single(x => x.ApplicationId == application2.Id);
            var riskapp3 = riskDb.RiskApplications.Single(x => x.ApplicationId == application3.Id);

            riskapp1.SignedOn = DateTime.UtcNow;
            riskapp2.SignedOn = DateTime.UtcNow;
            riskapp3.SignedOn = DateTime.UtcNow;
            riskDb.SubmitChanges();

            //lets try to create the 4th one and see the failure
            var customer4 = CustomerBuilder.New().WithEmployer(TestMask).WithFlatInAddress(flat).
                   WithHouseNumberInAddress(houseNumber).WithStreetInAddress(street).WithHouseNameInAddress(houseName).
                   WithDistrictInAddress(district).WithTownInAddress(town).WithCountyInAddress(county).WithPostcodeInAddress(postcode).Build();
            var application4 = ApplicationBuilder.New(customer4).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application4.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.TooManyLoansAtAddress));
        }
	}
}
