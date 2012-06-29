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
        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoanMobile()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journey.Teleport<AcceptedPageMobile>() as AcceptedPageMobile;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();
        }

        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoanMobileDropOff()
        {
            string email = Get.RandomEmail();
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var processing = journey.Teleport<ProcessingPageMobile>() as ProcessingPageMobile;
            
            //navigate to Login before Processing page 
            processing.Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "login");
            var loginPage = processing.WaitForPage<LoginPageMobile>() as LoginPageMobile;
            var summaryPage = loginPage.LoginAs(email, Get.GetPassword());
            var applyPage = summaryPage.ApplyForLoan("600", "20");
            var processingPage = applyPage.ClickApplyNowButton();
            
            //accept Loan and Sign documents
            var accepted = processingPage.WaitFor<AcceptedPageMobile>() as AcceptedPageMobile;
            accepted.SignAgreementConfirm();
            accepted.SignDirectDebitConfirm();
            var dealDonePage = accepted.Submit();

        }

		[Test, AUT(AUT.Za)]
        public void ZaDeclinedLoanMobile()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPageMobile>() as DeclinedPageMobile;

        }

        [Test, AUT(AUT.Uk), Pending("")]
        public void UkL0Journey()
        {
            var journey = JourneyFactory.GetL0Journey(Client.MobileHome());
            var mySummary = journey.Teleport<MySummaryPageMobile>() as MySummaryPageMobile;
        }

    }
}
