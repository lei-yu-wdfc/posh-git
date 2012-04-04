using System;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Api;
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
        private IWebElement TopupRequestPageSliders { get; set; }
        public TopupRequestPage(UiClient client) : base(client)
        {
            _submitButton = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageSubmitButton));
            _informativeBox = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageInformativeBox));
            _interestAndFees = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewInterestAndFees));
            _grandTotal = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewGrandTotal));
            _topupAmount = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewCreditRequest));   
        }

        public bool IsTopupRequestPageInformativeBoxDisplayed()
        {
            _informativeBox = Client.Driver.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageInformativeBox));
            return _informativeBox.Displayed;
        }
        
        public void SubmitButtonClick()
        {
            _submitButton.Click();
        }

        public String GetTotalToRepay()
        {
            _grandTotal = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewGrandTotal));
            return _grandTotal.Text;
        }

        public String GetInterestAndFees()
        {
            _interestAndFees = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewInterestAndFees));
            return _interestAndFees.Text;
        }

        public String GetTopupAmount()
        {
            _topupAmount = Content.FindElement(By.CssSelector(Ui.Get.TopupRequestPage.TopupRequestPageNewCreditRequest));
            return _topupAmount.Text;
        }
/*
        public void IsTopupRequestPageSliderReturningCorrrectValuesOnChange(string applicationId)
        {
            var topupRequestAmount = 50;
            //TopupRequestPage.TopupRequestPageSliders.HowMuch = topupRequestAmount;
            
            //Expected values
           _response = Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery {ApplicationId = applicationId, TopupAmount = topupRequestAmount});

            var totalRepayable = _response.Values["TotalRepayable"].Single();
            var interestAndFees = _response.Values["InterestAndFeesAmount"].Single();
            
            //check the output matches the returned values for 50 quid topupRequestAmount
            Assert.AreEqual(expectedValue: TopupRequestPage.GetTotalToRepay().Remove(0, 1), actualValue: totalRepayable);
            Assert.AreEqual(expectedValue: TopupRequestPage.GetTopupAmount().Remove(0, 1), actualValue: topupRequestAmount);
            Assert.AreEqual(expectedValue: TopupRequestPage.GetInterestAndFees().Remove(0, 1), actualValue: interestAndFees);
        }
 */

    }

}