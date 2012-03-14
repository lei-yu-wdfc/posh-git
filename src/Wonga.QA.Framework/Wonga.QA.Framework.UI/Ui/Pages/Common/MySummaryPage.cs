using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MySummaryPage : BasePage
    {
        public MyAccountNavigationElement Navigation { get; set; }
        
        public MySummaryPage(UiClient client) : base(client)
        {
            Navigation = new MyAccountNavigationElement(this);
            Assert.IsTrue(IsMySummaryTitleExists());
            
        }
        private bool IsMySummaryTitleExists()
        {

            try
            {
                var mySummaryTitle = Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.Title));
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }
    }
}