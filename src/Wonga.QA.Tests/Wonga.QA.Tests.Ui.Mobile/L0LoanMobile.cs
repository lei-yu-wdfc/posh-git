using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile;
using Wonga.QA.Framework.Mobile.Journey;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    [TestFixture]
    public class L0LoanMobile : UiMobileTest
    {
        [Test, AUT(AUT.Za), Pending("Waiting for mobile journeys")]
        public void ZaAcceptedLoanMobile()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journey.Teleport<AcceptedPageMobile>() as AcceptedPageMobile;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();
        }

        [Test, AUT(AUT.Za), Pending("Waiting for mobile journeys")]
        public void ZaAcceptedLoanMobileDropOff()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journey.Teleport<AcceptedPageMobile>() as AcceptedPageMobile;
            //write drop off code

        }

		[Test, AUT(AUT.Za), Pending("Waiting for mobile journeys")]
        public void ZaDeclinedLoanMobile()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPageMobile>() as DeclinedPageMobile;

        }

    }
}
