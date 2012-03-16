using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointApplicantIsSolventTest : BaseCheckpointTest
	{
		private const RiskMask TestMask = RiskMask.TESTIsSolvent;

		[Test, AUT(AUT.Uk), JIRA("UK-854")]
		public void AcceptForNormalCustomer()
		{
			RunSingleWorkflowTest(TestMask, new CustomerKathleenUk(), CheckpointDefinitionEnum.CustomerIsSolvent, CheckpointStatus.Verified);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-854")]
		public void DeclineForSolvent()
		{
			RunSingleWorkflowTest(TestMask, new CustomerLauraUk(), CheckpointDefinitionEnum.CustomerIsSolvent, CheckpointStatus.Failed);
		}
	}
}
