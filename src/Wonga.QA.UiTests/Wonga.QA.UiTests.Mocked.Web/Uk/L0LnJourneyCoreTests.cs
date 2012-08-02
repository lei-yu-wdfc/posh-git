using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [TestFixture, SUT(SUT.WIP), Parallelizable(TestScope.Self), Description("Main UI tests for UK")]
    public class L0LnJourneyCoreTests:UiTest
    {
        private string _email;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _email = Get.RandomEmail();
            Console.WriteLine("email={0}", _email);
        }

        [Test, AUT(AUT.Uk), Description("Check WIP IS mocked. The test runs only in WIP."), Owner(Owner.StanDesyatnikov)]
        public void WipIsMocked()
        {
            var homePage = Client.Home();
            Assert.IsTrue(homePage.IsMocked(), "WIP is not mocked");
        }

        [Test, AUT(AUT.Uk), Owner(Owner.StanDesyatnikov)]
        public void L0Journey()
        {
            var loginPage = Client.Login();
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(_email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), Owner(Owner.StanDesyatnikov)]
        public void LnJourney()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

    }
}
