using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.CL.uk
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	public class ApplicationReadinessWbTests : RiskServiceTestClUkBase
	{
		[Test]
		public void ApplicationIsReadyIfAllDataIsReceived()
		{
			SetupLegitCustomer();
			RunL0Journey();
			AssertVerificationStarted();
		}
	}
}
