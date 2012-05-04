using System;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class TopupRequestPage : BasePage
    {
        private IWebElement _submitButton;
        private IWebElement _informativeBox;
        private ApiResponse _response;
        private IWebElement _interestAndFees;
        private IWebElement _grandTotal;
        private IWebElement _topupAmount;
        private SmallTopupSlidersElement Sliders { get; set; }
        public TopupRequestPage(UiClient client) : base(client)
        {
            _submitButton = Content.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageSubmitButton));
            _informativeBox = Content.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageInformativeBox));
            _interestAndFees = Content.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageNewInterestAndFees));
            _grandTotal = Content.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageNewGrandTotal));
            _topupAmount = Content.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageNewCreditRequest));   
        }

        public bool IsTopupRequestPageInformativeBoxDisplayed()
        {
            _informativeBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.TopupRequestPage.TopupRequestPageInformativeBox));
            return _informativeBox.Displayed;
        }
        
        public void SubmitButtonClick()
        {
            _submitButton.Click();
            //return new TopupProcessingPage(Client);
        }
       
        public void IsTopupRequestPageSliderReturningCorrrectValuesOnChange(string applicationId)
        {
            const string topupRequestAmount = "50";
            Sliders = new SmallTopupSlidersElement(this);
            Sliders.HowMuch = topupRequestAmount;
            
            //Expected values
            var api = new ApiDriver();
            _response = api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery
                                 {ApplicationId = applicationId, TopupAmount = topupRequestAmount});

            var totalRepayable = _response.Values["TotalRepayable"].Single();
            var interestAndFees = _response.Values["InterestAndFeesAmount"].Single();
            
            //check the output matches the returned values for 50 quid topupRequestAmount
            Assert.AreEqual(Decimal.Parse(Sliders.GetSubTotal.Remove(0, 1)),Decimal.Parse(totalRepayable));
            Assert.AreEqual(Decimal.Parse(Sliders.GetTotalFees.Remove(0, 1)),Decimal.Parse(interestAndFees));
        }
    }
}