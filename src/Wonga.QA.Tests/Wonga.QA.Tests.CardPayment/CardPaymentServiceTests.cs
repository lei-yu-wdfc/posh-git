using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CardPayment
{
    [Parallelizable(TestScope.All)]
    public class CardPaymentServiceTests
    {
        [Test, AUT(AUT.Uk, AUT.Wb)]
        public void CardPaymentServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.CardPayment.IsRunning());
        }
    }
}
