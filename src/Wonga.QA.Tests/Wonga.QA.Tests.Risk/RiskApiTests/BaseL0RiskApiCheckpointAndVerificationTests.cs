using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class BaseL0RiskApiCheckpointAndVerificationTests : BaseRiskApiCheckpointAndVerificationTests
	{
		protected Application L0ApplicationWithSingleCheckPointAndVerifications(CustomerBuilder customerBuilder, CheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames)
		{
			if (_builderConfig == null)
			{
				//use the default
				_builderConfig = new ApplicationBuilderConfig();
			}

			CheckpointStatus expectedStatus = GetExpectedCheckpointStatus(_builderConfig.ExpectedDecisionStatus);

			Customer cust = customerBuilder.Build();

			Application app = ApplicationBuilder.New(cust)
				.WithIovationBlackBox(_builderConfig.IovationBlackBox.ToString())
				.WithExpectedDecision(_builderConfig.ExpectedDecisionStatus).Build();

			AssertCheckpointAndVerifications(expectedStatus, expectedVerificationNames, checkpointDefinition, app);

			return app;
		}

		protected Application L0ApplicationWithSingleCheckPointAndSingleVerification(CustomerBuilder customerBuilder, CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
		{
			return L0ApplicationWithSingleCheckPointAndVerifications(customerBuilder, checkpointDefinition, new[] { expectedVerificationName });
		}

		protected Application L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
		{
			return L0ApplicationWithSingleCheckPointAndVerifications(checkpointDefinition, new List<string> { expectedVerificationName });
		}

		protected Application L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames, string employerNameTestMask)
		{
			return L0ApplicationWithSingleCheckPointAndVerifications(
				CustomerBuilder.New().WithEmployer(employerNameTestMask),
				checkpointDefinition, expectedVerificationNames);
		}

		protected Application L0ApplicationWithSingleCheckPointAndVerifications(CheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames)
		{
			return L0ApplicationWithSingleCheckPointAndVerifications(
						checkpointDefinition,
						expectedVerificationNames,
						GetEmployerNameMaskFromCheckpointDefinition(checkpointDefinition));
		}
	}
}
