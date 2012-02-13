using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [Parallelizable(TestScope.All)]
    public class SalesforceServiceTests
    {
        [Test, AUT]
        public void SalesforceServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Salesforce.IsRunning());
        }
    }
}
