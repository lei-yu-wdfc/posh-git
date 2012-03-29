using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	public abstract class BaseCheckpointTest
	{
        protected RiskApplicationEntity CreateRiskApplicationUsingApi(RiskMask testMask, ICustomerData customerData)
		{
			var customer =
				CustomerBuilder.New()
				.WithEmployer(testMask)
				.WithForename(customerData.ForeName)
				.WithSurname(customerData.SurName)
				.WithDateOfBirth(customerData.DateOfBirth)
				.WithPostcodeInAddress(customerData.Postcode)
				.WithHouseNameInAddress(customerData.HouseName)
				.WithHouseNumberInAddress(customerData.HouseNumber)
				.WithFlatInAddress(customerData.Flat)
				.Build();

			var application = ApplicationBuilder.New(customer).Build();

			// wait until risk application will be created
			Do.Until(() => Drive.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id));

			return Drive.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id);
		}

        protected Application RunSingleWorkflowTest(RiskMask testMask, ICustomerData customerData, RiskCheckpointDefinitionEnum checkpoint, RiskCheckpointStatus expectedStatus)
		{
			var customer =
				CustomerBuilder.New()
				.WithEmployer(testMask)
				.WithForename(customerData.ForeName)
				.WithSurname(customerData.SurName)
//				.WithDateOfBirth(customerData.DateOfBirth) // This allow to avoid validation error in COMMS, because customer equal by Forename and DOB
				.Build();

			var application = ApplicationBuilder.New(customer)
                .WithExpectedDecision(expectedStatus == RiskCheckpointStatus.Verified ? ApplicationDecisionStatus.Accepted : ApplicationDecisionStatus.Declined)
				.Build();

			// wait until workflow will be created and started
			Do.Until(() => Drive.Db.Risk.RiskWorkflows.Single(x => x.ApplicationId == application.Id));

			var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id);
			Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");

			// wait until decision made
			Do.Until(() => Drive.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id && x.Decision != 0));

			Assert.Contains(
				Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, expectedStatus),
				Get.EnumToString(checkpoint));

			return application;
		}
	}
}
