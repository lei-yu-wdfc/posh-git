using System;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;


namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [Parallelizable(TestScope.All), SUT(SUT.WIP)]
    internal class ExtensionTests : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-427", "UK-1627", "UK-1746", "UKWEB-911")]
        [Pending("Waiting for refactored 'Mock Settings' popup window in WIP")]
        public void ExtensionJourneyPass()
        {
            string email = Get.RandomEmail();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            SelectMockedScenario("Scenario 3");

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

        private void SelectMockedScenario(string scenarionName)
        {
            IWebElement mockedScenariosMenu = Client.Driver.FindElement(By.Id("staticmock-trigger"));
            mockedScenariosMenu.Click();
            IWebElement mockedScenariosDropBox = Client.Driver.FindElement(By.Id("edit-user-status"));
            mockedScenariosDropBox.Click();
            //Do.With.Message("Mocked Scenarios Drop Box is not displayed").Timeout(new TimeSpan(0,0,3)).Until(() => mockedScenariosDropBox.SelectOption(option => option.Trim() == scenarionName));
            mockedScenariosDropBox.SelectOption(option => option.Trim() == scenarionName);
            Client.Driver.FindElement(By.Id("edit-submit-1")).Submit();
        }
    }
}
