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

        public IWebElement RepayButton;
        
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

        public string GetMyAccountStatus()
        {
            return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.AccountStatusText)).Text;
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
                var mySummaryTitle = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.Title));
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }
        public String GetTotalToRepay
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.TotalToRepay)).Text;  }
        }
        public String GetRepaymentDate
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.RepaymentDate)).Text; }
        }

        public String GetPromisedRepayAmount
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PromisedRepayAmount)).Text; }
        }
        public String GetPromisedRepayDate
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PromisedRepayDate)).Text; }
        }

        public void RepayButtonClick()
        {
            RepayButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.RepayButton)); 
            RepayButton.Click();
        }

        public void ChangePromiseDateButtonClick()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.ChangePromiseDateButton)).Click();
        }

        public String GetTotalToRepayAmountPopup
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.TotalToRepayAmountPopup)).Text; }
        }
        public String GetPromisedRepayDatePopup
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PromisedRepayDatePopup)).Text; }
        }

        public bool IsTagCloudAvailable()
        {
            try
            {
                var TagCloudText = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.TagCloud)).Text;
            }

            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public String GetTagCloud
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.TagCloud)).Text; }
        }

        public void ClickViewLoanDetailsButton()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.ViewLoanDetailsButton)).Click();
        }

        public void WaitForMySummaryPopup()
        {
            var popup = Do.With.Interval(10).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PopupForm)));
            Do.Until(() => popup.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PopupMySummaryTitle)).Displayed);
        }

        public bool IsPopupContainsSummaryDetailsTable()
        {
            var popup = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PopupForm));
            return popup.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.PopupSummaryDetailsTable)).Displayed;
        }

        public bool IsLoanStatusMessageAvailable()
        {
            try
            {
                var loanStatusMessageText = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.LoanStatusMessage)).Text;
            }

            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public String GetLoanStatusMessage
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.LoanStatusMessage)).Text; }

        }

        public void CheckScenarioElementsExist()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.Promise));
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.YouCan));
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.IntroText));
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.StatusMessage));
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.OptionsCloud));

        }

        public String GetIntroText
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.IntroText)).Text; }
        }

        public bool IsBackEndScenarioCorrect(int scenarioId)
        {
            try
            {
                var scenario = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.IntroText)).GetAttribute("summary-intro-text scenario-" + scenarioId.ToString("#")); 
            }
               
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }

        public String GetMaxAvailableCredit
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.MaxAvailableCredit)).Text; }
        }

        public void IsPrepaidCardButtonExist()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.MyAccountNavigationSection.MyPrepaidCardPageDetails));                 
        }

        public RepaymentOptionsPage RepayClick()
        {
            RepayButton = Client.Driver.FindElement(By.CssSelector(UiMap.Get.MySummaryPage.RepayButton)); 
            RepayButton.Click();
            return new RepaymentOptionsPage(Client);
        }

    }
}
