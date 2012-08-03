using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Ui.Validators;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FAAboutYouPage : BasePage
    {
        private readonly IWebElement _agreementReference;
        private readonly IWebElement _emailAddress;
        private IWebElement _editname;
        private IWebElement _editsurname;
        private IWebElement _editDOBday;
        private IWebElement _editDOBmonth;
        private IWebElement _editDOByear;
        private IWebElement _edithouseNumber;
        private IWebElement _editpostCode;
        private IWebElement _editemployer;
        private IWebElement _edityourHousehold;
        private IWebElement _editchildrenInHousehold;
        private IWebElement _editnumberOfVehiles;
        private readonly IWebElement _buttonPrevious;
        private readonly IWebElement _buttonNext;
        private IWebElement _postCodeError;

        public FAAboutYouPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            _agreementReference =
                Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.AgreementReference));
            _emailAddress = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.EmailAddress));
            _editname = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.Name));
            _editsurname = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.Surname));
            _editDOBday = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.DateOfBirthDay));
            _editDOBmonth = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.DateOfBirthMonth));
            _editDOByear = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.DateOfBirthYear));
            _edithouseNumber = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.HouseNumber));
            _editpostCode = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.PostCode));
            _editemployer = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.Employer));
            _edityourHousehold =
                Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.YourHousehold));
            _editchildrenInHousehold =
                Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.ChildrenInHousehold));
            _editnumberOfVehiles =
                Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.NumberOfVehiles));
            _buttonPrevious =
                Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.ButtonPrevious));
            _buttonNext = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.ButtonNext));
        }

        public string FirstName
        {
            get { return _editname.GetValue(); }
            set
            {
                _editname.Clear();
                _editname.SendKeys(value);
            }
        }

        public string LastName
        {
            get { return _editsurname.GetValue(); }
            set
            {
                _editsurname.Clear();
                _editsurname.SendKeys(value);
            }
        }

        public string DateOfBirth
        {
            get
            {
                String outDate = _editDOByear.GetValue() + "-";
                switch (_editDOBmonth.GetValue())
                {
                    case "Jan": { outDate += "01-"; break; }
                    case "Feb": { outDate += "02-"; break; }
                    case "Mar": { outDate += "03-"; break; }
                    case "Apr": { outDate += "04-"; break; }
                    case "May": { outDate += "05-"; break; }
                    case "Jun": { outDate += "06-"; break; }
                    case "Jul": { outDate += "07-"; break; }
                    case "Aug": { outDate += "08-"; break; }
                    case "Sep": { outDate += "09-"; break; }
                    case "Oct": { outDate += "10-"; break; }
                    case "Now": { outDate += "11-"; break; }
                    case "Dec": { outDate += "12-"; break; }
                }
                return outDate + int.Parse(_editDOBday.GetValue()).ToString("00");
            }
            set
            {
                _editDOBday.SendKeys(int.Parse(value.Split('-')[2]).ToString());
                switch (int.Parse(value.Split('-')[1]))
                {
                    case 1: { _editDOBmonth.SendKeys("Jan"); break; }
                    case 2: { _editDOBmonth.SendKeys("Feb"); break; }
                    case 3: { _editDOBmonth.SendKeys("Mar"); break; }
                    case 4: { _editDOBmonth.SendKeys("Apr"); break; }
                    case 5: { _editDOBmonth.SendKeys("May"); break; }
                    case 6: { _editDOBmonth.SendKeys("Jun"); break; }
                    case 7: { _editDOBmonth.SendKeys("Jul"); break; }
                    case 8: { _editDOBmonth.SendKeys("Aug"); break; }
                    case 9: { _editDOBmonth.SendKeys("Sep"); break; }
                    case 10: { _editDOBmonth.SendKeys("Oct"); break; }
                    case 11: { _editDOBmonth.SendKeys("Now"); break; }
                    case 12: { _editDOBmonth.SendKeys("Dec"); break; }
                }
                _editDOByear.SendKeys(value.Split('-').First());
            }
        }

        public string HouseNumber
        {
            get { return _edithouseNumber.GetValue(); }
            set { _edithouseNumber.SendValue(value); }
        }

        public string PostCode
        {
            get { return _editpostCode.GetValue(); }
            set{ _editpostCode.SendValue(value); }
        }

        public string Employer
        {
            get { return _editemployer.GetValue(); }
            set { _editemployer.SendValue(value); }
        }

        public string AdultsInHousehold
        {
            get { return _edityourHousehold.GetValue(); }
            set { _edityourHousehold.SendValue(value); }
        }

        public string ChidrenInHousehold
        {
            get { return _editchildrenInHousehold.GetValue(); }
            set { _editchildrenInHousehold.SendValue(value); }
        }

        public string NumberOfVehiles
        {
            get { return _editnumberOfVehiles.GetValue(); }
            set { _editnumberOfVehiles.SendValue(value); }
        }

        public string GetPrepopulatedAgreementReference()
        {
            return _agreementReference.GetValue();
        }

        public string GetPrepopulatedEmailAddress()
        {
            return _emailAddress.GetValue();
        }

        public BasePage PreviousClick()
        {
            _buttonPrevious.Click();
            return new FinancialAssessmentPage(Client);
        }

        public BasePage NextClick(bool error = false)
        {
            _buttonNext.Click();
            if (error)
            {
                var validator = new ValidatorBuilder().WithoutErrorsCheck().Build();
                return new FAAboutYouPage(Client, validator);
            }
            return new FAIncomePage(Client);
        }

        public bool PostCodeErrorPresent()
        {
            try
            {
                _postCodeError =
                    Do.Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.PostCodeError)));
                if (_postCodeError.Text == "")
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
