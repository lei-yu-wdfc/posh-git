using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    [TestFixture]
    public class L0LoanMobile : UiMobileTest
    {
        [Test, AUT(AUT.Za), Pending("Shouldn't run as part of Backend Tests")]
        public void ZaAcceptedLoanMobile()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();
        }

        [Test, AUT(AUT.Za), Pending("Shouldn't run as part of Backend Tests")]
        public void ZaAcceptedLoanMobileDropOff()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            //write drop off code

        }

		[Test, AUT(AUT.Za), Pending("Shouldn't run as part of Backend Tests")]
        public void ZaDeclinedLoanMobile()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;

        }

    }
}
