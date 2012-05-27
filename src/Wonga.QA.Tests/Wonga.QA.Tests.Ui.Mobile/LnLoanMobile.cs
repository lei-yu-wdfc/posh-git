using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Tests.Ui.Mobile
{
    class LnLoanMobile : UiMobileTest
    {
        [Test, Pending("Test not yet complete")]
        public void FullLnMobile()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New()
                                        .WithEmailAddress(email)
                                        .Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            loginPage.LoginAsMobile(email);

            var journey = JourneyFactory.GetLnJourney(Client.MobileHome());
            var page = journey.ApplyForLoan(200, 10)
                           .FillApplicationDetails()
                           .WaitForAcceptedPage()
                           .FillAcceptedPage()
                           .GoToMySummaryPage()
                           .CurrentPage as MySummaryPage;
        }
    }
}
