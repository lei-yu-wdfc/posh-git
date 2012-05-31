using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.Meta
{
    [TestFixture, Parallelizable(TestScope.Self), DependsOn(typeof(ColdStartTests))]
    public class LnJourneyTests : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-1533")]
        public void L0LnJourneyTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journeyL0.ApplyForLoan(200, 10)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage();

            var customer = new Customer(Guid.Parse(Drive.Api.Queries.Post(new GetAccountQuery { Login = email, Password = Get.GetPassword() }).Values["AccountId"].Single()));
            var application = customer.GetApplication();

            // Repay
            application.RepayOnDueDate();

            // Ln journey
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = journey.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }

        [Test, AUT(AUT.Uk)]
        public void CustomerApplicationBuildersWork()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            var loanAmount = 100;

            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(3).Build();

            var mySummaryPage = loginPage.LoginAs(email);

            // Check the actual text
            string actualTotalToRepay = mySummaryPage.GetTotalToRepay;
            Assert.IsTrue(actualTotalToRepay != "£0.00", "TotalToRepay should not be £0.");
        }
    }
}
