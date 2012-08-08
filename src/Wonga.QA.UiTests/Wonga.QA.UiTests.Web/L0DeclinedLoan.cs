using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    public class L0DeclinedLoan : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca)]
        public void DeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
        }
    }
}
