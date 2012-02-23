using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [Parallelizable(TestScope.All)]
    public class BankGatewayServiceTests
    {
        [Test]
        public void BankGatewayServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.BankGateway.IsRunning());

            switch (Config.AUT)
            {
                case AUT.Uk:
                    Assert.IsTrue(Driver.Svc.Hsbc.IsRunning());
                    break;
                case AUT.Za:
                    Assert.IsTrue(Driver.Svc.Hyphen.IsRunning());
                    break;
                case AUT.Ca:
                    Assert.IsTrue(Driver.Svc.Scotia.IsRunning());
                    break;
                case AUT.Wb:
                    break;
            }
        }
    }
}
