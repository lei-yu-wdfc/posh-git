using System;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;
using Wonga.QA.UiTests.Mocked.Web.Uk;


namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [Parallelizable(TestScope.All), SUT(SUT.WIP)]
    internal class ExtensionTests : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-427", "UK-1627", "UK-1746", "UKWEB-911"), Owner(Owner.StanDesyatnikov)]
        public void ExtensionJourneyPass()
        {
            string email = Get.RandomEmail();
            Client.Driver.Manage().Window.Maximize();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            var helper = new MockHelpers();
            helper.SelectMockedScenario(Client, "Scenario3");

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);
            
            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;
            agreementPage.Accept();

            var dealDonePage = new ExtensionDealDonePage(this.Client);
            Assert.IsFalse(dealDonePage.IsDealDonePageExtensionAmountNotPresent());
            Assert.IsFalse(dealDonePage.IsDealDonePageDateTokenPresent());
        }
    }
}
