﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NHamcrest.Core;
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

    public class AddressPopUpElement : BasePopUpElement
    {

        private readonly IWebElement _editFlat;
        private readonly IWebElement _editHouseNumber;
        private readonly IWebElement _editStreet;
        private readonly IWebElement _editDistrict;

        private readonly IWebElement _editTown;
        private readonly IWebElement _editCounty;
        private readonly IWebElement _editPostCode;
        private readonly IWebElement _editAddressPeriod;
        private readonly IWebElement _update;

        public AddressPopUpElement(BasePageMobile page) : base(page)
        {
            _editFlat = PopUp.FindElement(By.CssSelector("#edit-flat-za"));
            _editHouseNumber = PopUp.FindElement(By.CssSelector("#edit-house-number-za"));
            _editStreet = PopUp.FindElement(By.CssSelector("#edit-street-za"));
            _editDistrict = PopUp.FindElement(By.CssSelector("#edit-district-za"));
            _editTown = PopUp.FindElement(By.CssSelector("#edit-town-za"));
            _editCounty = PopUp.FindElement(By.CssSelector("#edit-county-za"));
            _editPostCode = PopUp.FindElement(By.CssSelector("#edit-postcode-za"));
            _editAddressPeriod = PopUp.FindElement(By.CssSelector("#edit-address-period-za"));
            _update = PopUp.FindElement(By.CssSelector("#edit-submit"));
        }

        public MyPersonalDetailsPageMobile EditAddress()
        {
            _editFlat.SendValue(Get.RandomInt(50).ToString(CultureInfo.InvariantCulture));
            _editHouseNumber.SendValue(Get.RandomString(10));
            _editStreet.SendValue(Get.RandomString(10));
            _editDistrict.SendValue(Get.RandomString(10));
            _editTown.SendValue("Cape Town");
            _editCounty.SendValue("Za");
            _editPostCode.SendValue("6865");
            _editAddressPeriod.SelectOption("2 to 3 years");
            _update.Click();
            var successPopUp = Do.Until(() => new SuccessPopUpElement(Page, "Thank you, your address has been updated."));
            successPopUp.Close();
            return Do.Until(() => new MyPersonalDetailsPageMobile(Page.Client));
        }
    }

    public class SuccessPopUpElement : BasePopUpElement
    {
        private readonly IWebElement _popUpMessage;

        public SuccessPopUpElement(BasePageMobile page, String message)
            : base(page)
        {
            Assert.IsTrue(PopUpTitle.Equals("Success"));
            _popUpMessage = PopUp.FindElement(By.CssSelector(".form-markup-wrapper>p"));
            Assert.That(_popUpMessage.Text,  Is.EqualTo(message));
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
            Assert.IsTrue(PopUpTitle.Equals("Update my password"));
            _currentPassword = PopUp.FindElement(By.CssSelector("#edit-current-password"));
            _editPassword = PopUp.FindElement(By.CssSelector("#edit-password"));
            _editPasswordConfirm = PopUp.FindElement(By.CssSelector("#edit-password-confirm"));
            _update = PopUp.FindElement(By.CssSelector("#edit-submit"));
        }

        public MyPersonalDetailsPageMobile EditPassword(string currentPassword, string newPassword)
        {
            _currentPassword.SendKeys(currentPassword);
            _editPassword.SendKeys(newPassword);
            _editPasswordConfirm.SendKeys(newPassword);
            _update.Click();
            var successPopUp = Do.Until(() => new SuccessPopUpElement(Page, "Your password has been updated."));
            successPopUp.Close();
            return Do.Until(() => new MyPersonalDetailsPageMobile(Page.Client));
        }
    }

    public class EditPhoneNumberPopUpElement : BasePopUpElement
    {

        private readonly IWebElement _homePhone;
        private readonly IWebElement _mobilePhone;
        private readonly IWebElement _update ;
        
        public EditPhoneNumberPopUpElement(BasePageMobile page) : base(page)
        {
            Assert.IsTrue(PopUpTitle.Equals("Change my phone numbers"));
            _homePhone = PopUp.FindElement(By.CssSelector("#edit-home-phone-za"));
            _mobilePhone = PopUp.FindElement(By.CssSelector("#edit-mobile-phone-za"));
            _update = PopUp.FindElement(By.CssSelector("#edit-submit"));
        }

        public MyPersonalDetailsPageMobile UpdateHomePhoneNumber(string number)
        {
            _homePhone.SendValue(number);
            _update.Click();
            var successPopUp = Do.Until(() => new SuccessPopUpElement(Page, "Your home phone number was updated."));
            successPopUp.Close();
            return Do.Until(() => new MyPersonalDetailsPageMobile(Page.Client));
        }

        public MyPersonalDetailsPageMobile UpdateMobilePhoneNumber(string number)
        {
            _mobilePhone.SendValue(number);
            _update.Click();
            var changeMyPhoneNUmbersPopUp = Do.Until(() => new ChangeMyPhoneNmmbersPopUpElement(Page));
            var successPopUp = changeMyPhoneNUmbersPopUp.EnterPinAndUpdate();
            successPopUp.Close();
            return Do.Until(() => new MyPersonalDetailsPageMobile(Page.Client));
        }
    }

    public class ChangeMyPhoneNmmbersPopUpElement : BasePopUpElement
    {
        private readonly IWebElement _homePhoneWrapper;
        private readonly IWebElement _mobilePhoneWrapper;
        private readonly IWebElement _wongaTip;
        private readonly  IWebElement _pin ;
        private readonly IWebElement _update;

        public ChangeMyPhoneNmmbersPopUpElement(BasePageMobile page) : base(page)
        {
            Assert.IsTrue(PopUpTitle.Equals("Change my phone number(s)"));
            _homePhoneWrapper = PopUp.FindElement(By.CssSelector("#edit-home-phone-za-wrapper"));
            _mobilePhoneWrapper = PopUp.FindElement(By.CssSelector("#edit-mobile-phone-za-wrapper"));
            _wongaTip = PopUp.FindElement(By.CssSelector(".wonga-tip"));
            _pin = PopUp.FindElement(By.CssSelector("#edit-mobile-pin"));
            _update = PopUp.FindElement(By.CssSelector("#edit-submit"));
        }

        public SuccessPopUpElement EnterPinAndUpdate()
        {
            _pin.SendValue("0000");
            _update.Click();
            return Do.Until(() => new SuccessPopUpElement(Page, "Thank you: your telephone number has been updated."));
        }
    }

    public class MyLoanDetailsPopUpElement : BasePopUpElement
    {
        private readonly IWebElement _borrowed;
        private readonly IWebElement _owed;
        private readonly IWebElement _breakdown;
        private readonly IWebElement _chart;

        public MyLoanDetailsPopUpElement(BasePageMobile page) : base(page)
        {
            Assert.IsTrue(PopUpTitle.Equals("My Loan Details"));
            _chart = PopUp.FindElement(By.CssSelector("#wonga-chart>img"));
            _borrowed = PopUp.FindElement(By.CssSelector(".loan-details-intro-borrowed"));
            _owed = PopUp.FindElement(By.CssSelector(".loan-details-intro-owed"));
            _breakdown = PopUp.FindElement(By.CssSelector(".breakdown"));
        }
    }
}
