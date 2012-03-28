using System;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MySummaryPage : BasePage, IApplyPage 
    {
        public MyAccountNavigationElement Navigation { get; set; }
        public SlidersElement Sliders { get; set; }
        public IWebElement _repayButton { get; set; }
        
        public MySummaryPage(UiClient client) : base(client)
        {
            
            switch(Config.AUT)
            {
                case (AUT.Za) :
                    Navigation = new MyAccountNavigationElement(this);
                    break;

                case (AUT.Ca) :
                    Navigation = new MyAccountNavigationElement(this);
                    LookForSliders();
                    break;

            }
                
            
            
            Assert.IsTrue(IsMySummaryTitleExists());
            
        }
        public bool LookForSliders()
        {
            try
            {
                Sliders = new SlidersElement(this);
                return true;
            }
            catch(NoSuchElementException)
            {
                return false;
            }
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
        public String GetTotalToRepay
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.TotalToRepay)).Text;  }
        }
        public String GetRepaymentDate
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.RepaymentDate)).Text; }
        }

        public String GetPromisedRepayAmount
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PromisedRepayAmount)).Text; }
        }
        public String GetPromisedRepayDate
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PromisedRepayDate)).Text; }
        }

        public void RepayButtonClick()
        {
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.RepayButton)).Click();
        }

        public String GetTotalToRepayAmountPopup
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.TotalToRepayAmountPopup)).Text; }
        }
        public String GetPromisedRepayDatePopup
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PromisedRepayDatePopup)).Text; }
        }
    }
}