using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class BaseLnRiskApiCheckpointAndVerificationTests : BaseRiskApiCheckpointAndVerificationTests
	{
        protected Application LNApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName)
		{
			return LNApplicationWithSingleCheckPointAndSingleVerification(
						checkpointDefinition, expectedVerificationName,
						GetEmployerNameMaskFromCheckpointDefinition(checkpointDefinition));
		}

        protected Application LNApplicationWithSingleCheckPointAndSingleVerification(RiskCheckpointDefinitionEnum checkpointDefinition, string expectedVerificationName, string employerNameTestMask)
		{
			return LNApplicationWithSingleCheckPointAndVerifications(checkpointDefinition, new[] { expectedVerificationName }, employerNameTestMask);
		}

        protected Application LNApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames)
		{
			return LNApplicationWithSingleCheckPointAndVerifications(
						checkpointDefinition, expectedVerificationNames,
						GetEmployerNameMaskFromCheckpointDefinition(checkpointDefinition));
		}

        protected Application LNApplicationWithSingleCheckPointAndVerifications(RiskCheckpointDefinitionEnum checkpointDefinition, IEnumerable<string> expectedVerificationNames, string employerNameTestMask)
		{
			if (_builderConfig == null)
			{
				//use default
				_builderConfig = new ApplicationBuilderConfig();
			}


			Application firstApplication = MakeFirstLNApplicationAndRepay();

			//tweak the test mask
			EmploymentDetailEntity employmentDetails = Drive.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == firstApplication.GetCustomer().Id);
			employmentDetails.EmployerName = employerNameTestMask;
			employmentDetails.Submit();

            RiskCheckpointStatus expectedStatus = GetExpectedCheckpointStatus(_builderConfig.ExpectedDecisionStatus);

			Application secondApplication = ApplicationBuilder.New(firstApplication.GetCustomer())
				.WithIovationBlackBox(_builderConfig.IovationBlackBox.ToString())
				.WithExpectedDecision(_builderConfig.ExpectedDecisionStatus).Build();

			AssertCheckpointAndVerifications(expectedStatus, expectedVerificationNames, checkpointDefinition, secondApplication);

			return secondApplication;
		}

		protected Application MakeFirstLNApplicationAndRepay()
		{
			Customer customer = CustomerBuilder.New().Build();

            Application application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            application.RepayOnDueDate();

			return application;
		}

		

	}
}
