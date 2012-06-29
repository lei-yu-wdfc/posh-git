using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
{
    public class BasePopUpElement : BaseElement
    {
        public IWebElement PopUp;
        public string PopUpTitle;

        public BasePopUpElement(BasePageMobile page) :base(page)
        {
            PopUp = page.Client.Driver.FindElement(By.CssSelector("div#fancybox-content"));
            PopUpTitle = PopUp.FindElement(By.CssSelector(".modal h1")).Text;
        }

        public void Close()
        {
            Page.Client.Driver.FindElement(By.CssSelector("div#fancybox-wrap a#fancybox-close")).Click();
        }
    }

    public class SuccessPopUpElement : BasePopUpElement
    {
        public SuccessPopUpElement(BasePageMobile page)
            : base(page)
        {
            Assert.IsTrue(PopUpTitle.Equals("Success"));
        }
    }

    public class EditPasswordPopUpElement : BasePopUpElement
    {
        private readonly IWebElement _currentPassword;
        private readonly IWebElement _editPassword;
        private readonly IWebElement _editPasswordConfirm;
        private readonly IWebElement _update;


        public EditPasswordPopUpElement(BasePageMobile page)
            : base(page)
        {
            Assert.IsTrue(PopUpTitle.Equals("Change your password"));
            _currentPassword = PopUp.FindElement(By.CssSelector("#edit-current-password"));
            _editPassword = PopUp.FindElement(By.CssSelector("#edit-password"));
            _editPasswordConfirm = PopUp.FindElement(By.CssSelector("#edit-password-confirm"));
            _update = PopUp.FindElement(By.CssSelector("#edit-submit"));
        }

        public void EditPassword(string currentPassword, string newPassword)
        {
            _currentPassword.SendKeys(currentPassword);
            _editPassword.SendKeys(newPassword);
            _editPasswordConfirm.SendKeys(newPassword);
            _update.Click();
            var successPopUp = Do.Until(() => new SuccessPopUpElement(Page));
            successPopUp.Close();
        }
    }
}
