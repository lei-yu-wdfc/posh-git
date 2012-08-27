using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.PayLater
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    public class ApplicationReadinessTests : RiskServiceTestPayLaterUkBase
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