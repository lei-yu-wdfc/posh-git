using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Application = Wonga.QA.Framework.Application;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class SalesForceTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-220"), Pending("Problem with presents customers in SF")]
        public void CustomerInformationDisplayInSF()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            Thread.Sleep(30000);
            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.SalesforceUi.Username, Config.SalesforceUi.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);
            Thread.Sleep(2000);
            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-220"), Pending("right now can't do this")]
        public void CustomerGetsAcceptedDecisionHasRecordWithLiveLoanStatusInSF()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            Thread.Sleep(30000);
            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.SalesforceUi.Username, Config.SalesforceUi.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);
            Thread.Sleep(2000);
            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());
            salesForceSearchResultPage.GoToCustomerDetailsPage();
            // Chek customer has a record with "LiveLoan" status in SF
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-220"), Pending("right now can't do this")]
        public void CustomerGetsAcceptedDecisionLoanAmountIndicatesLoanAmountCustomerAppliedForDueDays()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            Thread.Sleep(30000);
            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.SalesforceUi.Username, Config.SalesforceUi.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);
            Thread.Sleep(2000);
            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());
            salesForceSearchResultPage.GoToCustomerDetailsPage();
            // "Loan amount" indicates loan amount customer applied for. Due days = Due date. Check: Charges, today's balance, final interest, final balance.
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-220"), Pending("right now can't do this")]
        public void CustomerTakesLoanCheckBalanceAtTodayAndBalanceAtTodayShouldBePrincipalPlusFee()
        {
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            Thread.Sleep(30000);
            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.SalesforceUi.Username, Config.SalesforceUi.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);
            Thread.Sleep(2000);
            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());
            salesForceSearchResultPage.GoToCustomerDetailsPage();
            //Check Balance at today and balance at today should be Principal+Fee (+Interest if Loan was taken more than 3 days ago)
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-222")]
        public void CustomerGetsAcceptedDecisionDontClickAcceptButtonApplicationStatusInSF()
        {
            string email = Get.RandomEmail();
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journey.ApplyForLoan(200, 10)
                                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                                .FillAddressDetails()
                                .FillAccountDetails()
                                .FillBankDetails()
                                .WaitForAcceptedPage().CurrentPage as AcceptedPage;
            Thread.Sleep(10000);
            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.SalesforceUi.Username, Config.SalesforceUi.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);
            Thread.Sleep(2000);
            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());
            var customerDetailPage = salesForceSearchResultPage.GoToCustomerDetailsPage();


        }
    }
}
