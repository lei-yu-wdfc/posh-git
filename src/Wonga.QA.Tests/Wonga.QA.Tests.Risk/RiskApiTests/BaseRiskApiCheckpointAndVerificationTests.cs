using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	public class BaseRiskApiCheckpointAndVerificationTests
	{
		public const string RiskIovationResponseTimeoutSecondsKeyName = "Risk.IovationResponseTimeoutSeconds";

		protected ApplicationBuilderConfig _builderConfig;


		#region assertions

		protected void AssertCheckpointAndVerifications1900126404(CheckpointStatus expectedStatus, IEnumerable<string> expectedVerificationNames, CheckpointDefinitionEnum checkpoint, Application application)
		{
			//first check what was the failed checkpoint if expected status is declined			
			AssertFailedCheckpointOnApplication(application, expectedStatus, checkpoint);

			RiskApplicationEntity riskApplication = new DbDriver().Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);

			AssertCheckpointOnApplicationDbEntity(expectedStatus, checkpoint, riskApplication);

			AssertVerificationsOnApplicationDbEntity(expectedVerificationNames, riskApplication);
		}

		protected void AssertVerificationsOnApplicationDbEntity1384639980(IEnumerable<string> expectedVerificationNames, RiskApplicationEntity riskApplication)
		{
			Assert.AreEqual(expectedVerificationNames.Count(), riskApplication.WorkflowVerifications.Count());

			foreach (string expectedVerificationName in expectedVerificationNames)
			{
				Assert.IsTrue(
					riskApplication.WorkflowVerifications.Any(v => v.VerificationDefinitionEntity.Name == expectedVerificationName),
					String.Format("Verification name should be {0}", expectedVerificationName));
			}
		}

		protected static void AssertCheckpointOnApplicationDbEntity(CheckpointStatus expectedStatus, CheckpointDefinitionEnum checkpoint, RiskApplicationEntity riskApplication)
		{
			var dbCheckpoint = riskApplication.WorkflowCheckpoints.Single(r => r.RiskApplicationId == riskApplication.RiskApplicationId);

			Assert.AreEqual(Convert.ToByte(expectedStatus), dbCheckpoint.CheckpointStatus);

			string checkpointName = dbCheckpoint.CheckpointDefinitionEntity.Name;

			Assert.AreEqual(Data.EnumToString(checkpoint), checkpointName);
		}

		/// <summary>
		/// this method asserts on what was returned on the API call for the failed checkpoit. Only applicable for declined applications.
		/// </summary>
		/// <param name="application">the application object</param>
		/// <param name="expectedFailedCheckpoint">the expected checkpoint to fail</param>
		protected static void AssertFailedCheckpointOnApplication(Application application, CheckpointDefinitionEnum? expectedFailedCheckpoint)
		{
			if (expectedFailedCheckpoint.HasValue)
			{
				Assert.AreEqual(Data.EnumToString(expectedFailedCheckpoint), application.FailedCheckpoint);
			}
			else
			{
				Assert.IsNull(application.FailedCheckpoint);
			}
		}

		protected static void AssertFailedCheckpointOnApplication(Application application, CheckpointStatus expectedStatus, CheckpointDefinitionEnum checkpoint)
		{
			AssertFailedCheckpointOnApplication(application,
			                                    expectedStatus == CheckpointStatus.Failed
			                                    	? checkpoint
			                                    	: (CheckpointDefinitionEnum?)null);
		}


		protected void AssertCheckpointAndVerifications(CheckpointStatus expectedStatus, IEnumerable<string> expectedVerificationNames, CheckpointDefinitionEnum checkpoint, Application application)
		{
			//first check what was the failed checkpoint if expected status is declined			
			AssertFailedCheckpointOnApplication(application, expectedStatus, checkpoint);

			RiskApplicationEntity riskApplication = new DbDriver().Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);

			AssertCheckpointOnApplicationDbEntity(expectedStatus, checkpoint, riskApplication);

			AssertVerificationsOnApplicationDbEntity(expectedVerificationNames, riskApplication);
		}

		protected void AssertVerificationsOnApplicationDbEntity(IEnumerable<string> expectedVerificationNames, RiskApplicationEntity riskApplication)
		{
			Assert.AreEqual(expectedVerificationNames.Count(), riskApplication.WorkflowVerifications.Count());

			foreach (string expectedVerificationName in expectedVerificationNames)
			{
				Assert.IsTrue(
					riskApplication.WorkflowVerifications.Any(v => v.VerificationDefinitionEntity.Name == expectedVerificationName),
					String.Format("Verification name should be {0}", expectedVerificationName));
			}
		}

		#endregion

		#region helper methods

		/// <summary>
		/// changes configuration for iovation timeout and returns current value
		/// </summary>
		/// <param name="seconds">new value for timeout in secs</param>
		/// <returns>current timeout value in secs</returns>
		protected static int SetRiskIovationResponseTimeoutSeconds(int seconds)
		{
			var serviceConfigurationEntity = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == RiskIovationResponseTimeoutSecondsKeyName);
			var currentValue = serviceConfigurationEntity.Value;
			serviceConfigurationEntity.Value = seconds.ToString();
			serviceConfigurationEntity.Submit();
			return Int32.Parse(currentValue);
		}

		protected static int SetIovationMockWaitTimeSecondsForMockResponse(IovationMockResponse response, int seconds)
		{
			string iovationType = response.ToString();
			var iovationDataOutput = Driver.Db.QAData.IovationDataOutputs.Single(io => io.Type == iovationType);
			var currentValue = iovationDataOutput.WaitTimeInSeconds;
			iovationDataOutput.WaitTimeInSeconds = seconds;
			iovationDataOutput.Submit();

			return currentValue;
		}

		protected string GetEmployerNameMaskFromCheckpointDefinition(CheckpointDefinitionEnum checkpointDefinition)
		{
			var className = GetCheckpointClassName(checkpointDefinition);

			return string.Format("test:{0}", className);
		}

		//TODO: check why this does not work for middle name???
		protected string GetMiddleNameMaskFromCheckpointDefinition(CheckpointDefinitionEnum checkpointDefinition)
		{
			var className = GetCheckpointClassName(checkpointDefinition);

			return string.Format("TEST{0}", className);
		}

		protected static string GetCheckpointClassName(CheckpointDefinitionEnum checkpointDefinition)
		{
			//type name has this format:
			//"Wonga.Risk.Checkpoints.ApplicationElementNotOnCSBlacklist, Wonga.Risk.Checkpoints, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
			var checkpoint = new DbDriver().Risk.CheckpointDefinitions.SingleOrDefault(cd => cd.Name == Data.EnumToString(checkpointDefinition));
			if (checkpoint == null)
			{
				throw new ArgumentOutOfRangeException("checkpointDefinition", checkpointDefinition, "Not found in DB");
			}

			string[] assemblyTokens = checkpoint.TypeName.Split(',');
			if (assemblyTokens.Length < 1)
			{
				throw new ArgumentOutOfRangeException("checkpointDefinition", checkpointDefinition,
													  string.Format("Invalid Checkpoint TypeName in DB: {0}", checkpoint.TypeName));
			}

			//1st elem is the full namespace of type
			string typeFullName = assemblyTokens[0];
			string[] namespaceTokens = typeFullName.Split('.');

			//last elem is the className... mask is Test:className
			string className =
				namespaceTokens.Length == 0
					? typeFullName
					: namespaceTokens.Last();
			return className;
		}

		protected static CheckpointStatus GetExpectedCheckpointStatus(ApplicationDecisionStatusEnum decision)
		{
			switch (decision)
			{
				case ApplicationDecisionStatusEnum.Declined:
					return CheckpointStatus.Failed;

				case ApplicationDecisionStatusEnum.Pending:
				case ApplicationDecisionStatusEnum.WaitForData:

					return CheckpointStatus.Pending;

				case ApplicationDecisionStatusEnum.Accepted:
				case ApplicationDecisionStatusEnum.ReadyToSign:
					return CheckpointStatus.Verified;

				default:
					throw new InvalidEnumArgumentException("decision", (int)decision, decision.GetType());
			}
		}
		#endregion
	}
}
