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

namespace Wonga.QA.Tests.Ui.Region.Uk
{
    [TestFixture, Parallelizable(TestScope.Self), Description("Main UI tests for UK"), Importance(Importance.Critical)]
    public class UkUiMetaTests:UiTest
    {
        private string _email;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _email = Get.RandomEmail();
            Console.WriteLine("email={0}", _email);
        }

        [Test, AUT(AUT.Uk)]
        public void L0JourneyTest()
        {
            var loginPage = Client.Login();
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(_email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), DependsOn("L0JourneyTest")]
        public void RepaymentOnDueDateTest()
        {
            // Get Applicaiton object to do repayment
            var customer = new Customer(Guid.Parse(Drive.Api.Queries.Post(new GetAccountQuery { Login = _email, Password = Get.GetPassword() }).Values["AccountId"].Single()));
            var application = customer.GetApplication();

            // Repay
            application.RepayOnDueDate();
        }

        [Test, AUT(AUT.Uk), DependsOn("RepaymentOnDueDateTest")]
        public void LnJourneyTest()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }
    }
}
