﻿using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	public abstract class BaseCheckpointTest
	{

		protected RiskApplicationEntity CreateRiskApplicationUsingApi(string testMask, ICustomerData customerData)
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
			Do.Until(() => Driver.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id));

			return Driver.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id);
		}

		protected Application RunSingleWorkflowTest(string testMask, ICustomerData customerData, CheckpointDefinitionEnum checkpoint, CheckpointStatus expectedStatus)
		{
			var customer =
				CustomerBuilder.New()
				.WithEmployer(testMask)
				.WithForename(customerData.ForeName)
				.WithSurname(customerData.SurName)
				.WithDateOfBirth(customerData.DateOfBirth)
				.Build();

			var application = ApplicationBuilder.New(customer)
				.WithExpectedDecision(expectedStatus == CheckpointStatus.Verified ? ApplicationDecisionStatusEnum.Accepted : ApplicationDecisionStatusEnum.Declined)
				.Build();

			// wait until workflow will be created and started
			Do.Until(() => Driver.Db.Risk.RiskWorkflows.Single(x => x.ApplicationId == application.Id));

			var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
			Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");

			// wait until decision made
			Do.Until(() => Driver.Db.Risk.RiskApplications.Single(x => x.ApplicationId == application.Id && x.Decision != 0));

			Assert.Contains(
				Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, expectedStatus),
				Data.EnumToString(checkpoint));

			return application;
		}

	}
}
