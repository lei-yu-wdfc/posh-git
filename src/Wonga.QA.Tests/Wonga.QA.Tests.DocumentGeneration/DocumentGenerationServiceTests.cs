using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.DocumentGeneration
{
    [Parallelizable(TestScope.All)]
    public class DocumentGenerationServiceTests
    {
        [Test, AUT(AUT.Wb, AUT.Za, AUT.Uk, AUT.Ca)]
        public void DocumentGenerationServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.DocumentGeneration.IsRunning());
        }
    }
}
