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
        public TabsElement Tabs { get; set; }
        public TopupSlidersElement TopupSliders { get; set; }
        
        public MySummaryPage(UiClient client) : base(client)
        {
            
            switch(Config.AUT)
            {
                case (AUT.Za) :
                    Navigation = new MyAccountNavigationElement(this);
                    Tabs = new TabsElement(this);
                    break;

                case (AUT.Ca) :
                    Navigation = new MyAccountNavigationElement(this);
                    Tabs = new TabsElement(this);
                    LookForSliders();
                    break;
                case AUT.Wb:
                    Navigation = new MyAccountNavigationElement(this);
                    Tabs = new TabsElement(this);
                    break;

                case (AUT.Uk):
                    Navigation = new MyAccountNavigationElement(this);
                    Tabs = new TabsElement(this);
                    LookForSliders();
                    LookForTopupSliders();
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

        public bool LookForTopupSliders()
        {
            try
            {
                TopupSliders = new TopupSlidersElement(this);
                return true;
            }
            catch (NoSuchElementException)
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

        public bool IsTagCloudAvailable()
        {
            try
            {
                var TagCloudText = Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.TagCloud)).Text;
            }

            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public String GetTagCloud
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.TagCloud)).Text; }
        }

        public void ClickViewLoanDetailsButton()
        {
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.ViewLoanDetailsButton)).Click();
        }

        public void WaitForMySummaryPopup()
        {
            var popup = Do.With.Interval(10).Until(() => Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PopupForm)));
            Do.Until(() => popup.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PopupMySummaryTitle)).Displayed);
        }

        public bool IsPopupContainsSummaryDetailsTable()
        {
            var popup = Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PopupForm));
            return popup.FindElement(By.CssSelector(Ui.Get.MySummaryPage.PopupSummaryDetailsTable)).Displayed;
        }

        public bool IsLoanStatusMessageAvailable()
        {
            try
            {
                var loanStatusMessageText = Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.LoanStatusMessage)).Text;
            }

            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public String GetLoanStatusMessage
        {
            get { return Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.LoanStatusMessage)).Text; }

        }

        public void CheckScenarioElementsExist()
        {
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.Promise));
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.YouCan));
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.IntroText));
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.StatusMessage));
            Client.Driver.FindElement(By.CssSelector(Ui.Get.MySummaryPage.OptionsCloud));

        }
    }
}
