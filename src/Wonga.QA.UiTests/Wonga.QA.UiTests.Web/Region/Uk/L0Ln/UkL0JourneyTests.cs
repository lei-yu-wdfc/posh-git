﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Uk.L0Ln
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    public class UkL0JourneyTests : UiTest
    {
        // Check L0 loan is accepted and Loan Agreement is displayed
        // Check L0 loan is completed and text on Deal Done page is correct
        [Test, JIRA("UK-730", "UK-731"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        public void L0AcceptedCompleted()
        {
            string expectedDealDoneText = "Your application has been accepted\r\nThe cash will be winging its way into your bank account in the next 15 minutes! Please just be aware that different banks take different lengths of time to show new deposits.\r\nPlease don\'t forget that you have promised to repay on {repay date} when you\'ll need to have £{repay amount} ready in the bank account linked to your debit card. You can login to your Wonga account at any time to keep track of your loan, apply for more cash (depending on your trust rating) and even extend or repay early.\r\nWe hope you find the money useful and, if you love our service, please now check out the options below!";
            const int loanAmount = 100;
            const int days = 10;
            string paymentAmount = 115.91M.ToString("#.00");
            DateTime paymentDate = DateTime.Now.AddDays(days);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAmount(loanAmount).WithDuration(days);

            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            Assert.IsTrue(acceptedPage.IsAgreementFormDisplayed());

            var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;
            string actualDealDoneText = dealDonePage.GetDealDonePageText;
            expectedDealDoneText = expectedDealDoneText.Replace("{repay date}", Date.GetOrdinalDate(paymentDate, "dddd d MMM yyyy")).Replace("{repay amount}", paymentAmount);
            Assert.AreEqual(expectedDealDoneText, actualDealDoneText);
        }

        [Test, JIRA("UKWEB-253"), Owner(Owner.StanDesyatnikov)]
        public void L0Declined()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithAmount(400).WithDuration(30)
                .WithDeclineDecision();
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }

        [Test, JIRA("UK-969", "UKWEB-250"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Pending ("UKWEB-1143: Document links are not working in Account Setup Page")]
        public void L0AccountSetupPageDocumentLinksShouldOpenPopups()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            int loanAmount = 200;
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithAmount(loanAmount)
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithEmail(email);
            var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
            Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
            Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

            Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
            accountSetupPage.ClosePopupWindow();

            Thread.Sleep(1000);

            Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
            accountSetupPage.ClosePopupWindow();

            Thread.Sleep(1000);

            accountSetupPage.ClickSecciLink();
            Assert.Contains(accountSetupPage.SecciPopupWindowContent(), loanAmount.ToString("#"));
            accountSetupPage.ClosePopupWindow();
        }
    }
}
