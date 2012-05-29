using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
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
            var journey = JourneyFactory.GetLNJourneyWB(Client.Home());
            journey.ApplyForLoan(5000, 15)
                        .ApplyNow()
                        .WaitForApplyTermsPage()
                        .ApplyTerms()
                        .FillAcceptedPage()
                        .GoHomePage();
        }

        [Test, AUT(AUT.Wb), Pending("Test not yet complete")]
        public void WbDeclinedLnLoan()
        {

            var loginPage = Client.Login();
            loginPage.LoginAs("qa.wonga.com+QB-WK-156-77b99268-ff34-451e-a067-4b0c48f3c5ac@gmail.com");
            var journey = JourneyFactory.GetLNJourneyWB(Client.Home());
            var declinedPage = journey.ApplyForLoan(5000, 15)
                                            .ApplyNow()
                                            .WaitForDeclinedPage();
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
