using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.ColdStorage
{
    [Parallelizable(TestScope.All)]
    public class ColdStorageServiceTests
    {
        [Test, AUT(AUT.Wb, AUT.Uk, AUT.Ca)]
        public void ColdStorageServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.ColdStorage.IsRunning());
        }
    }
}
