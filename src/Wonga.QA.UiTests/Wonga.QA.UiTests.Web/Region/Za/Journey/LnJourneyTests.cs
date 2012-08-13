using System;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Za.Journey
{
    [TestFixture, Parallelizable(TestScope.All), AUT(AUT.Za)]
    internal class LnJourneyTests : UiTest
    {
        [Test, JIRA("QA-196"), Pending("ZA-2510"), Category(TestCategories.SmokeTest)]
        public void LnCustomerTakesNewLoanAndChangesTheMobilePhoneThenChangesShouldBeReflected()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            var oldMobilePhone = myPersonalDetails.GetMobilePhone;
            var homePage = Client.Home();

            var journey = JourneyFactory.GetLnJourney(homePage);
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;
            applyPage.SetNewMobilePhone = "0111111111";
            applyPage.ApplicationSection.SetPin = "0000";
            journey.CurrentPage = applyPage.Submit() as ProcessingPage;
            mySummaryPage = journey.Teleport<MySummaryPage>() as MySummaryPage;
            myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            Assert.AreNotEqual(oldMobilePhone, myPersonalDetails.GetMobilePhone);
            Assert.AreEqual("0111111111", myPersonalDetails.GetMobilePhone);
            var mobileFromDb =
                Drive.Data.Comms.Db.CustomerDetails.FindAllBy(AccountId: customer.Id).FirstOrDefault().MobilePhone;

            Assert.AreEqual("0111111111", mobileFromDb);
        }

        [Test, JIRA("QA-198")]
        public void LnCustomerChangesMobilePhoneAndEntersInvalidPinShouldNotBeAbleToTakeLoan()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPage = loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;

            applyPage.SetNewMobilePhone = "0111111111";
            applyPage.ApplicationSection.SetPin = "1111";
            try
            {
                applyPage.Submit();
            }
            catch (AssertionFailureException exception)
            {
                Assert.IsTrue(exception.Message.Contains("The SMS PIN you entered was incorrect"));
            }
        }

        [Test]
        public void ZaFullLnJourneyTest()
        {
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
