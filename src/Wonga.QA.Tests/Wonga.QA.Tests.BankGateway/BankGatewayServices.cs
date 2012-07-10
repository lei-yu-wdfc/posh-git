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
                    Assert.IsTrue(Drive.Svc.BankGatewayHsbc.IsRunning());
                    break;
                case AUT.Za:
                    Assert.IsTrue(Drive.Svc.BankGatewayHyphen.IsRunning());
                    Assert.IsTrue(Drive.Svc.BankGatewayEasyPay.IsRunning());
                    break;
                case AUT.Ca:
                    Assert.IsTrue(Drive.Svc.BankGatewayScotia.IsRunning());
                    Assert.IsTrue(Drive.Svc.BankGatewayBmo.IsRunning());
                    Assert.IsTrue(Drive.Svc.BankGatewayRbc.IsRunning());
                    break;
                case AUT.Wb:
                    break;
            }
        }
    }
}
