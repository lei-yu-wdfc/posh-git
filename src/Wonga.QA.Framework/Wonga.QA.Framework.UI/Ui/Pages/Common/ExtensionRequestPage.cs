using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ExtensionRequestPage : BasePage
    {
        private IWebElement _submitButton;
        private IWebElement _informativeBox;
        private IWebElement _card;
        private IWebElement _securityCode;
        private ApiResponse _response;
        private IWebElement _extensionRequestDate;
        //private IWebElement _interestAndFees;
        private IWebElement _grandTotal;
        //private IWebElement _extensionDuration;
        private SmallExtensionSlidersElement Sliders { get; set; }
        public ExtensionRequestPage(UiClient client) : base(client)
        {
            _submitButton = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageSubmitButton));
            _informativeBox = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageInformativeBox));
            _card = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageCard));
            _securityCode = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageSecurityCode));
            //_interestAndFees = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageInterestFees));
            _grandTotal = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageNewGrandTotal));   
        }

        public bool IsExtensionRequestPageInformativeBoxDisplayed()
        {
            _informativeBox = Client.Driver.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageInformativeBox));
            return _informativeBox.Displayed;
        }

        public bool IsExtensionRequestPageCardPresent()
        {
            _card = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageCard));
            IList<IWebElement> options = _card.FindElements(By.TagName("option"));
            if (options.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void SubmitButtonClick()
        {
            _submitButton.Click();
            //return new ExtensionProcessingPage(Client);
        }
       
        public void IsExtensionRequestPageSliderReturningCorrectValuesOnChange(string applicationId)
        {
            const string extensionRequestDuration = "2";
            _extensionRequestDate = Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageRepaymentDate));
            Sliders = new SmallExtensionSlidersElement(this);
            Sliders.HowLong = extensionRequestDuration;
            
            //Extract Requested Date from Page
            DateTime convertedDate = DateTime.Parse(_extensionRequestDate.Text);
            
            //Expected values
            var api = new ApiDriver();

            _response = api.Queries.Post(new GetFixedTermLoanExtensionCalculationQuery { ApplicationId = applicationId, ExtendDate = convertedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz") });

            var totalRepayable = _response.Values["NewFinalBalance"].Single();
            var newFees = _response.Values["Fee"].Single();
            
            //check the output matches the returned values for 50 quid extensionRequestAmount
            Assert.AreEqual(Decimal.Parse(Sliders.GetGrandTotal.Remove(0, 1)),Decimal.Parse(totalRepayable));
            Assert.AreEqual(Decimal.Parse(Sliders.GetNewFees.Remove(0, 1)),Decimal.Parse(newFees));
        }
    }
}