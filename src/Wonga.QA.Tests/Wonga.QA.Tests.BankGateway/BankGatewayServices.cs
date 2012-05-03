using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [Parallelizable(TestScope.All)]
    public class BankGatewayServices
    {
        [Test]
        public void BankGatewayServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.BankGateway.IsRunning());

            switch (Config.AUT)
            {
                case AUT.Uk:
                    Assert.IsTrue(Drive.Svc.Hsbc.IsRunning());
                    break;
                case AUT.Za:
                    Assert.IsTrue(Drive.Svc.Hyphen.IsRunning());
                    break;
                case AUT.Ca:
                    Assert.IsTrue(Drive.Svc.Scotia.IsRunning());
                    Assert.IsTrue(Drive.Svc.Bmo.IsRunning());
                    break;
                case AUT.Wb:
                    break;
            }
        }
    }
}
