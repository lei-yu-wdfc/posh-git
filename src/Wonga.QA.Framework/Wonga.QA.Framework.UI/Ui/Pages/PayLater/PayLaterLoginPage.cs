using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;



namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class PayLaterLoginPage : BasePage
    {
        private readonly IWebElement _inputEmailId;
        private readonly IWebElement _inputPassId;
        private IWebElement _submitButton;

        private readonly IWebElement _apr;
        private readonly IWebElement _forgottenPassword;
        private readonly IWebElement _singUp;

        private readonly IWebElement _fee;
        private readonly IWebElement _paymentAmount;

        public PayLaterLoginPage(UiClient client)
            : base(client)
        {
            _inputEmailId = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.InputEmailId));
            _inputPassId = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.InputPassId));
            _submitButton = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.SubmitButton));

            _apr = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.Apr));
            _forgottenPassword = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.ForgottenPassword));
            _singUp = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.SingUp));

            _fee = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.Fee));
            _paymentAmount = Content.FindElement(By.CssSelector(UiMap.Get.PayLaterLoginPage.PaymentAmount));
        }

        public SubmitionPage LoginAs(string username, string password)
        {
            _inputEmailId.Clear();
            _inputEmailId.SendKeys(username);
            _inputPassId.SendKeys(password);
            _submitButton.Click();
            
            //note: manual refrashing page
            var page = Client.PayLaterSubmition();
            //return "Login";
            return new SubmitionPage(Client);
        }

        public Dictionary<string, string> InspectElemrnts()
        {
            var elements = new Dictionary<string, string>
                               {
                                   {"Apr", _apr.Text},
                                   {"SingUp", _singUp.Text},
                                   {"ForgottenPassword", _forgottenPassword.Text},
                                   {"Fee", _fee.Text},
                                   {"PaymentAmount", _paymentAmount.Text}
                               };

            return elements;
        }
    }
}
