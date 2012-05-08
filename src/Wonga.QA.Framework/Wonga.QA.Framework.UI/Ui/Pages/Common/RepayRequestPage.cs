using System;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class RepayRequestPage : BasePage
    {
        private IWebElement _submitButton;
        private IWebElement _informativeBox;
        private ApiResponse _response;
        private IWebElement _repayAmount;
        private IWebElement _remainderAmount;
        private SmallRepaySlidersElement Sliders { get; set; }
        private IWebElement _cancelButton;
        private IWebElement _oweToday;
        private string _repayTotal;

        public RepayRequestPage(UiClient client) : base(client)
        {
            _submitButton = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageSubmitButton));
            _informativeBox = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageInformativeBox));
            _repayAmount = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageRepayAmount));
            _remainderAmount = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageRemainderAmount));
            _cancelButton = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageCancelButton));
            _oweToday = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageOweCurrently));
            _repayTotal = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageReadMeMessageRepayTotal)).Text;   
        }

        public bool IsTopupRequestPageInformativeBoxDisplayed()
        {
            _informativeBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageInformativeBox));
            return _informativeBox.Displayed;
        }
        
        public void SubmitButtonClick()
        {
            _submitButton.Click();
            //return new TopupProcessingPage(Client);
        }
       
        public void IsRepayRequestPageSliderReturningCorrectValuesOnChange(string applicationId)
        {
            const string repayRequestAmount = "50";
            DateTime todayDate = DateTime.Now;
            Sliders = new SmallRepaySlidersElement(this);
            Sliders.HowMuch = repayRequestAmount;
            
            //Expected values
            var api = new ApiDriver();
            _response = api.Queries.Post(new GetRepayLoanCalculationQuery
                                 {ApplicationId = applicationId, RepayAmount = repayRequestAmount, RepayDate = todayDate});

            var remainderAmount = _response.Values["TotalRepayableOnDueDate"].Single();
            
            //check the output matches the returned values for 50 quid repayRequestAmount
            Assert.AreEqual(Decimal.Parse(Sliders.GetRemainderTotal.Remove(0, 1)), Decimal.Parse(remainderAmount));
        }

        public void CancelButtonClick()
        {
            _cancelButton.Click();
        }

        public String OweToday
        {
            get { return _oweToday.Text; }
        }

        public String WantToRepayBox
        {
            get { return _repayAmount.GetValue();  }
            set { _repayAmount.SendValue(value); }
        }

        public String RemainderAmount
        {
            get { return _remainderAmount.Text; }
        }

        public String RepayTotal
        {
            get { return _repayTotal; }
        }
 
        
    }
}