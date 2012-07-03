using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    class LnLoanMobile : UiMobileTest
    {
        [Test, AUT(AUT.Za), Pending("Test not yet complete")]
        public void FullLnMobile()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New()
                                        .WithEmailAddress(email)
                                        .Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email, "Passw0rd");

            var journey = JourneyFactory.GetLnJourney(Client.MobileHome());
            var page = journey.Teleport<ProcessingPageMobile>() as ProcessingPageMobile;
        }
    }
}
