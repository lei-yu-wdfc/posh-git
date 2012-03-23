using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointTooManyLoansAtAddressTest : BaseCheckpointTest
	{
		private const RiskMask TestMask = RiskMask.TESTTooManyLoansAtAddress;

		[Test, AUT(AUT.Uk), JIRA("UK-913")]
		public void AcceptForOneAllication()
		{
            RunSingleWorkflowTest(TestMask, new CustomerJanetUk(), RiskCheckpointDefinitionEnum.TooManyLoansAtAddress, RiskCheckpointStatus.Verified);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-913")]
		public void DeclinedIfTooManyOpenLoans()
		{
			var custData1 = new CustomerKathleenUk();

			PreparePreviousLoans();

			RunSingleWorkflowTest(TestMask, 
				new CustomerJanetUk // override Address settings
					{
						Postcode = custData1.Postcode,
						Flat = custData1.Flat,
						HouseName = custData1.HouseName,
						HouseNumber = custData1.HouseNumber
					},
                RiskCheckpointDefinitionEnum.TooManyLoansAtAddress, RiskCheckpointStatus.Failed);
		}

		#region Implementation

		private void PreparePreviousLoans()
		{
			var app1 = CreateRiskApplicationUsingApi(TestMask, new CustomerKathleenUk());
			var app2 = CreateRiskApplicationUsingApi(TestMask, new CustomerKathleenUk { SurName = "Test" });

			// Sign Applications
			app1.SignedOn = DateTime.UtcNow;
			app2.SignedOn = DateTime.UtcNow;
			Drive.Db.Risk.SubmitChanges();

		}

		#endregion
	}
}
