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
        private IWebElement _editdateOfBirth;
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
            : base(client)
        {
            _agreementReference =
                Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.AgreementReference));
            _emailAddress = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.EmailAddress));
            _editname = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.Name));
            _editsurname = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.Surname));
            _editdateOfBirth = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.DateOfBirth));
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
            get { return _editdateOfBirth.GetValue(); }
            set
            {
                _editdateOfBirth.Clear();
                _editdateOfBirth.SendKeys(value);
            }
        }

        public string HouseNumber
        {
            get { return _edithouseNumber.GetValue(); }
            set
            {
                _edithouseNumber.Clear();
                _edithouseNumber.SendKeys(value);
            }
        }

        public string PostCode
        {
            get { return _editpostCode.GetValue(); }
            set
            {
                _editpostCode.Clear();
                _editpostCode.SendKeys(value);
            }
        }

        public string Employer
        {
            get { return _editemployer.GetValue(); }
            set
            {
                _editemployer.Clear();
                _editemployer.SendKeys(value);
            }
        }

        public string AdultsInHousehold
        {
            get { return _edityourHousehold.GetValue(); }
            set
            {
                _edityourHousehold.Clear();
                _edityourHousehold.SendKeys(value);
            }
        }

        public string ChidrenInHousehold
        {
            get { return _editchildrenInHousehold.GetValue(); }
            set
            {
                _editchildrenInHousehold.Clear();
                _editchildrenInHousehold.SendKeys(value);
            }
        }

        public string NumberOfVehiles
        {
            get { return _editnumberOfVehiles.GetValue(); }
            set
            {
                _editnumberOfVehiles.Clear();
                _editnumberOfVehiles.SendKeys(value);
            }
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
                _postCodeError = Content.FindElement(By.CssSelector(UiMap.Get.FinancialAssessmentAboutYouPage.PostCodeError));
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
