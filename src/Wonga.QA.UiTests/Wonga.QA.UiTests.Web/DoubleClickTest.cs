using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [TestFixture, Parallelizable(TestScope.All)]
    internal class DoubleClickTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-321")]
        public void l0JoureyThroughDoubleClickCookies()
        {

            var journeyL0 = JourneyFactory.GetL0Journey(Client.DoubleclickCookiesHome())
              .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var accountSetupPage = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-321")]
        public void l0JoureyDropOffThroughDoubleClickCookies()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.DoubleclickCookiesHome())
              .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journeyL0.Teleport<AcceptedPage>() as AcceptedPage;
            var home = Client.Home();
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-321")]
        public void lnJoureyThroughDoubleClickCookies()
        {
            Client.Driver.Navigate().GoToUrl(Config.Ui.DoubleClickCookiesHome);
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }
    }
}