using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.Core;

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
        private IWebElement _interestAndFees;
        private IWebElement _grandTotal;
        //private IWebElement _extensionDuration;
        private SmallExtensionSlidersElement Sliders { get; set; }
        public ExtensionRequestPage(UiClient client) : base(client)
        {
            _submitButton = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageSubmitButton));
            _informativeBox = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageInformativeBox));
            _card = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageCard));
            _securityCode = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageSecurityCode));
            _interestAndFees = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageInterestFees));
            _grandTotal = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageNewGrandTotal));   
        }

        public bool IsExtensionRequestPageInformativeBoxDisplayed()
        {
            _informativeBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageInformativeBox));
            return _informativeBox.Displayed;
        }

        public bool IsExtensionRequestPageCardPresent()
        {
            _card = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageCard));
            IList<IWebElement> options = _card.FindElements(By.TagName("option"));
            if (options.Count > 0)
            {
                return true;
            }
            return false;
        }
        
        public void SubmitButtonClick()
        {
            _submitButton.Click();
            //return new ExtensionProcessingPage(Client);
        }

        public void setSecurityCode(string code)
        {
          _securityCode.SendKeys(code);
        }
       
        public void IsExtensionRequestPageSliderReturningCorrectValuesOnChange(string applicationId)
        {
            const string extensionRequestDuration = "2";
            Sliders = new SmallExtensionSlidersElement(this);
            Sliders.HowLong = extensionRequestDuration;

            _extensionRequestDate = Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageRepaymentDate));
            var extensionRequestDate = _extensionRequestDate.Text.Replace("st", "").Replace("nd", "").Replace("rd", "").Replace("th", "");
                        
            //Extract Requested Date from Page
            DateTime convertedDate = DateTime.Parse(extensionRequestDate);
            var newConvertedDate = convertedDate.ToDate(DateFormat.Date);
            
            Console.WriteLine("hello {0}", convertedDate.ToString());
            //Expected values
            var api = new ApiDriver();
        
            _response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = applicationId });

            var sliderMinDays = _response.Values["SliderMinDays"].Single();
            var sliderMaxDays = _response.Values["SliderMaxDays"].Single();
            var totalDueToday = _response.Values["TotalAmountDueToday"].Single();
            var extensionPartPaymentAmount = _response.Values["ExtensionPartPaymentAmount"].Single();
            var currentPrincipleAmount = _response.Values["CurrentPrincipleAmount"].Single();
            var loanExtensionFee = _response.Values["LoanExtensionFee"].Single();

            var quotes = _response.Values["Quote"];
            
            //check the output matches the returned values for 50 quid extensionRequestAmount
            //Console.WriteLine("getGrandT {0}", Decimal.Parse(Sliders.GetGrandTotal.Remove(0, 1)).ToString());
            //Console.WriteLine("totalRep {0}", totalRepayable);
            
            //Assert.AreEqual(Decimal.Parse(Sliders.GetGrandTotal.Remove(0, 1)),Decimal.Parse(totalRepayable));
            //Assert.AreEqual(Decimal.Parse(Sliders.GetNewFees.Remove(0, 1)),Decimal.Parse(newFees));
        }    

        public String InformativeBox {
            get { return Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageExtensionDuration)).GetValue(); }
        }

        public void SetInformativeBox(int value)
        {
            Content.FindElement(By.CssSelector(Ui.Get.ExtensionRequestPage.ExtensionRequestPageExtensionDuration)).SendValue(value.ToString("#")); 
        }

        public String RepaymentDate {
            get { return Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageRepaymentDate)).Text; }
        }
        
        public String InterestFees {
            get { return Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageInterestFees)).Text; }
        }

        public String OweToday {
            get { return Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageOweToday)).Text; }
        }

        public String TotalRepayToday {
            get { return Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageTotalRepayToday)).Text; }
        }

        public String NewCreditAmount
        {
            get { return Content.FindElement(By.CssSelector(UiMap.Get.ExtensionRequestPage.ExtensionRequestPageNewCreditAmount)).Text; }
        }

    }
}