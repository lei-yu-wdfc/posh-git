using System;
using System.Threading;
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
        private IWebElement _card;
        private IWebElement _securityCode;
        private ApiResponse _response;
        private IWebElement _repayAmount;
        private IWebElement _remainderAmount;
        private SmallRepaySlidersElement Sliders { get; set; }
        private IWebElement _cancelButton;
        private IWebElement _oweToday;
        private string _repayTotal;
        private IWebElement _loanPeriodClarification;


        public RepayRequestPage(UiClient client) : base(client)
        {
            _submitButton = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageSubmitButton));
            _informativeBox = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageInformativeBox));
            _card = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageCard));
            _securityCode = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageSecurityCode));
            _repayAmount = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageRepayAmount));
            _remainderAmount = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageRemainderAmount));
            _cancelButton = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageCancelButton));
            _oweToday = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageOweCurrently));
            _repayTotal = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageReadMeMessageRepayTotal)).Text;
            _loanPeriodClarification = Content.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageLoanPeriodClarification));   
        }

        public bool IsRepayRequestPageInformativeBoxDisplayed()
        {
            _informativeBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.RepayRequestPage.RepayRequestPageInformativeBox));
            return _informativeBox.Displayed;
        }

        public void setSecurityCode(string code)
        {
            _securityCode.SendKeys(code);
        }

        public void SubmitButtonClick()
        {
            _submitButton.Click();
            //return new TopupProcessingPage(Client);
        }
       
        public void IsRepayRequestPageSliderReturningCorrectValuesOnChange(string applicationId, string repayRequestAmount)
        {
            //const string repayRequestAmount = "50";
            DateTime todayDate = DateTime.Now;
            Sliders = new SmallRepaySlidersElement(this);
            Sliders.HowMuch = repayRequestAmount;
            
            //Expected values
            var api = new ApiDriver();
            _response = api.Queries.Post(new GetRepayLoanCalculationQuery
                                 {ApplicationId = applicationId, RepayAmount = repayRequestAmount, RepayDate = todayDate});

            var remainderAmount = _response.Values["TotalRepayableOnDueDate"].Single();
            
            //check the output matches the returned values for repayRequestAmount
            Assert.AreEqual(Decimal.Parse(remainderAmount), Decimal.Parse(Sliders.GetRemainderTotal.Remove(0, 1)));
        }

        public void IsRepayRequestPageSliderReturningCorrectOverDueValuesOnChange(string applicationId, string repayRequestAmount)
        {
            //const string repayRequestAmount = "50";
            DateTime todayDate = DateTime.Now;
            Sliders = new SmallRepaySlidersElement(this);
            Sliders.HowMuch = repayRequestAmount;

            //Expected values
            var api = new ApiDriver();
            //_response = api.Queries.Post(new GetRepayLoanCalculationQuery { ApplicationId = applicationId, RepayAmount = repayRequestAmount, RepayDate = todayDate });
            _response = api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = applicationId });

            var totalOwed = _response.Values["SliderMaxAmount"].Single();
            var totalDec = Decimal.Parse(totalOwed);
            Decimal repayRequestAmountDec = Decimal.Parse(repayRequestAmount);
            //check the output matches the returned values for repayRequestAmount
            var sliderRemainder = Sliders.GetRemainderTotal.Remove(0, 1);
            var remainderAmount = Decimal.Parse(_response.Values["SliderMaxAmount"].Single()) - Decimal.Parse(repayRequestAmount);

            Assert.AreEqual(Decimal.Parse(remainderAmount), Decimal.Parse(Sliders.GetRemainderTotal.Remove(0, 1)));
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
            set { _repayAmount.SendValue(value); Thread.Sleep(2000); }
        }

        public String RemainderAmount
        {
            get { return _remainderAmount.Text; }
        }

        public String RepayTotal
        {
            get { return _repayTotal; }
        }

        public String LoanPeriodClarification
        {
            get { return _loanPeriodClarification.Text; }
        }

        public bool IsLoanPeriodClarificationDisplayed
        {
            get { return _loanPeriodClarification.Displayed; }
        }

        public string RepayCard
        {
            get { return _card.Text; }
        }
    }
}