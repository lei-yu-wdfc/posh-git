using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CallReport
{
    [Parallelizable(TestScope.All)]
    public class CallReportServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void CallReportServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.CallReport.IsRunning());
        }
    }
}
