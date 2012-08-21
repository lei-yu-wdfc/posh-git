using OpenQA.Selenium;
using Wonga.QA.Framework.UI;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    public class MockHelpers: UiTest
    {
        /// <summary>
        /// Selects a scenario in the Mock Settings menu
        /// </summary>
        /// <param name="scenarionName"></param>
        public void SelectMockedScenario(string scenarionName)
        {
            IWebElement mockedScenariosMenu = Client.Driver.FindElement(By.Id("staticmock-trigger"));
            mockedScenariosMenu.Click();
            IWebElement mockedScenariosDropBox = Client.Driver.FindElement(By.Id("edit-user-status"));
            mockedScenariosDropBox.SelectOption(scenarionName);
            Client.Driver.FindElement(By.XPath("//input[@value='Save settings']")).Click();
        }
    }
}
