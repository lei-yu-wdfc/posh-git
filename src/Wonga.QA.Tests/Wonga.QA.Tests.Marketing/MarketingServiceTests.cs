using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Marketing
{
    [Parallelizable(TestScope.All)]
    public class MarketingServiceTests
    {
        [Test, AUT(AUT.Ca, AUT.Pl, AUT.Uk, AUT.Za)]
        public void MarketingServiceIsRunning()
        {
              Assert.IsTrue(Drive.Svc.Marketing.IsRunning());
        }
    }
}
