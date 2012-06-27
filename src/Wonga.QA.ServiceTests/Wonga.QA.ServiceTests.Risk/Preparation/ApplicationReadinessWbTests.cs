using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.Preparation
{
	[Parallelizable(TestScope.All), AUT(AUT.Wb)]
	public class ApplicationReadinessWbTests : RiskServiceTestWbBase
	{
		[Test,AUT(AUT.Wb)]
		public void ApplicationIsReadyIfAllDataIsReceived()
		{
			SetupLegitCustomer(); 
			RunL0Journey();
			AssertVerificationStarted();
		}
	}
}
