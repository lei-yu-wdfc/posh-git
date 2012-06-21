using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    class WbLnJourneyTests: UiTest
    {

        [Test, AUT(AUT.Wb)]
        public void WbAcceptedLnLoan()
        {
            var loginPage = Client.Login();
            string email = CreateWbLnCustomer();

            loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(5000).WithDuration(15);
            var homePage = journey.Teleport<HomePage>() as HomePage;
        }

        [Test, AUT(AUT.Wb)]
        public void WbDeclinedLnLoan()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithMiddleName("Middle").WithEmailAddress(email).Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var applicationInfo =
                ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Declined).
                    Build() as BusinessApplication;

            loginPage.LoginAs(email);

            var lNjourney = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(5000).WithDuration(15);
            var declinedPage = lNjourney.Teleport<DeclinedPage>() as DeclinedPage;
        }

        public string CreateWbLnCustomer()
        {
            string email = Get.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var applicationInfo =
                ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                    Build() as BusinessApplication; 
            var paymentPlan = applicationInfo.GetPaymentPlan();

            var today = DateTime.UtcNow.Date;

            applicationInfo.MoveBackInTime(7, false);
            for (int i = 0; i < paymentPlan.NumberOfPayments; i++)
            {
                applicationInfo.MorningCollectionAttempt(paymentPlan, (i + 1) == paymentPlan.NumberOfPayments, true);
                applicationInfo.MoveBackInTime(7, true);
            }

            var totalOutstandingAmount = applicationInfo.GetTotalOutstandingAmount();

            Assert.AreEqual(0, totalOutstandingAmount);

            Do.With.Message("ClosedOn date should be set").Until(
                () => Drive.Data.Payments.Db.Applications.FindByExternalId(applicationInfo.Id).ClosedOn > today);

            return email;
        }
    }
}
