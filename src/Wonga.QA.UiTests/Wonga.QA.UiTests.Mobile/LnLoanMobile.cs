using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile;
using Wonga.QA.Framework.Mobile.Journey;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Mobile
{
    class LnLoanMobile : UiMobileTest
    {
        [Test, AUT(AUT.Za), Pending("Test is not yet complete")]
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
            var page = journey.Teleport<DealDonePage>() as DealDonePage;
        }

        [Test, AUT(AUT.Uk), Pending("Test not yet complete")]
        public void FullLnMobileUK()
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
            var page = journey.Teleport<DealDonePage>() as DealDonePage;
        }

        [Test, AUT(AUT.Uk), Pending("Test not yet complete")]
        public void TopupLoanTest()
        {

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).WithLoanAmount(100).WithLoanTerm(7).Build();
            var mySummary = loginPage.LoginAs(email, "Passw0rd");

            var topUpPage = mySummary.TopUpLoan("100");
            topUpPage.SubmitButtonClick();
            var topUpAcceptPage = Do.Until(() => new TopupAcceptPageMobile(Client));
            var dealDone = topUpAcceptPage.Accept();

        }

        [Test, AUT(AUT.Uk), Pending("Test not yet complete")]
        public void LoanExtensionTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).WithLoanAmount(100).WithLoanTerm(7).Build();

            loginPage.LoginAs(email, "Passw0rd");
            
           
        }
    }
}
