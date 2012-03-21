using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [Parallelizable(TestScope.All)]
    public class SalesforceServiceTests
    {
        [Test]
        public void SalesforceServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Salesforce.IsRunning());
        }
    }
}
