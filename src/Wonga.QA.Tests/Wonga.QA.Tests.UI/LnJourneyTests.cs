using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    class LnJourneyTests : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("QA-196")]
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
            var applyPage = journey.ApplyForLoan(200, 10)
                .SetName(name, surname).CurrentPage as ApplyPage;
            applyPage.SetNewMobilePhone = "0111111111";
            applyPage.ApplicationSection.SetPin = "0000";
            journey.CurrentPage = applyPage.Submit() as ProcessingPage;
            mySummaryPage = journey.WaitForAcceptedPage()
                                .FillAcceptedPage()
                                .GoToMySummaryPage()
                                .CurrentPage as MySummaryPage;
            myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            Assert.AreNotEqual(oldMobilePhone, myPersonalDetails.GetMobilePhone);
            Assert.AreEqual("0111111111", myPersonalDetails.GetMobilePhone);
            var mobileFromDb =
                Drive.Data.Comms.Db.CustomerDetails.FindAllBy(AccountId: customer.Id).FirstOrDefault().MobilePhone;

            Assert.AreEqual("0111111111", mobileFromDb);
        }

        [Test, AUT(AUT.Za), JIRA("QA-198")]
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
            var applyPage = journey.ApplyForLoan(200, 10).CurrentPage as ApplyPage;

            applyPage.SetNewMobilePhone = "0111111111";
            applyPage.ApplicationSection.SetPin = "1111";
            try
            {
                applyPage.Submit();
            }
            catch(AssertionFailureException exception)
            {
                Assert.IsTrue(exception.Message.Contains("The Pin was incorrect."));
            }
        }

        [Test, AUT(AUT.Ca), Pending("Example of CA Ln journey")]
        public void CaFullLnJourneyTest()
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
            loginPage.LoginAs(email);

            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.ApplyForLoan(200, 10)
                           .SetName(name, surname)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;


        }

        [Test, AUT(AUT.Za), Pending("Example of ZA Ln journey")]
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
            var page = journey.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }

        [Test, AUT(AUT.Uk), Pending("Example of uk Ln journey")]
        public void UkFullLnJourneyTest()
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
            var page = journey.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;            
        }

    }
}
