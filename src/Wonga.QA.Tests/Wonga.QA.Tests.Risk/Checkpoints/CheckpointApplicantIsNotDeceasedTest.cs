using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointApplicantIsNotDeceasedTest : BaseCheckpointTest
	{
	    private const RiskMask TestMask = RiskMask.TESTIsAlive;

		[Test, AUT(AUT.Uk), JIRA("UK-853")]
		public void AcceptForNormalCustomer()
		{
			RunSingleWorkflowTest(TestMask, new CustomerKathleenUk(), CheckpointDefinitionEnum.ApplicantIsAlive, CheckpointStatus.Verified);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-853")]
		public void DeclineForDeceased()
		{
			RunSingleWorkflowTest(TestMask, new CustomerKathleenUk { ForeName = "Johnny", SurName = "DeadGuy" }
				, CheckpointDefinitionEnum.ApplicantIsAlive, CheckpointStatus.Failed);
		}
	}
}
