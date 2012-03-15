using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All)]
	public class CheckpointApplicationElementNotCifasFlaggedTest : BaseCheckpointTest
	{
		private const string TestMask = "test:ApplicationElementNotCIFASFlagged";

		[Test, AUT(AUT.Uk), JIRA("UK-852")]
		public void AcceptNoCifas()
		{
			RunSingleWorkflowTest(TestMask, new CustomerJanetUk(), CheckpointDefinitionEnum.CIFASFraudCheck, CheckpointStatus.Verified);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-852")]
		public void DeclineCifasDetected()
		{
			var customerData = new CustomerJanetUk();
			customerData.ForeName = string.Format("{0}CIFAS", customerData.ForeName);
			RunSingleWorkflowTest(TestMask, customerData, CheckpointDefinitionEnum.CIFASFraudCheck, CheckpointStatus.Failed);
		}
	}
}
