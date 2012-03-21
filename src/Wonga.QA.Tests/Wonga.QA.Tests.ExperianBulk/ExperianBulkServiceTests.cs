using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.ExperianBulk
{
    [Parallelizable(TestScope.All)]
    public class ExperianBulkServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void ExperianBulkServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.ExperianBulk.IsRunning());
        }
    }
}
