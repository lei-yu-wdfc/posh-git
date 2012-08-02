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

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [TestFixture, Parallelizable(TestScope.Self), Description("Main UI tests for UK"), Category("CoreTest")]
    public class L0LnJourneyCoreTests:UiTest
    {
        private string _email;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _email = Get.RandomEmail();
            Console.WriteLine("email={0}", _email);
        }

        [Test, AUT(AUT.Uk), Owner(Owner.StanDesyatnikov)]
        public void L0JourneyTest()
        {
            var loginPage = Client.Login();
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(_email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), DependsOn("L0JourneyTest"), Owner(Owner.StanDesyatnikov)]
        public void RepaymentOnDueDateTest()
        {
            // Get Applicaiton object to do repayment
            var customer = new Customer(Guid.Parse(Drive.Api.Queries.Post(new GetAccountQuery { Login = _email, Password = Get.GetPassword() }).Values["AccountId"].Single()));
            var application = customer.GetApplication();

            // Repay
            application.RepayOnDueDate();
        }

        [Test, AUT(AUT.Uk), DependsOn("RepaymentOnDueDateTest"), Owner(Owner.StanDesyatnikov)]
        public void LnJourneyTest()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), Description("Check RC is NOT mocked. The test runs only in RC."), Owner(Owner.StanDesyatnikov)]
        [SUT(SUT.RC)]
        public void RcIsNotMocked()
        {
            var homePage = Client.Home();
            Assert.IsFalse(homePage.IsMocked(), "RC is mocked");
        }

        [Test, AUT(AUT.Uk), Description("Check WIP IS mocked. The test runs only in WIP."), Owner(Owner.StanDesyatnikov)]
        [SUT(SUT.WIP)]
        public void WipIsMocked()
        {
            var homePage = Client.Home();
            Assert.IsTrue(homePage.IsMocked(), "WIP is not mocked");
        }

    }
}
