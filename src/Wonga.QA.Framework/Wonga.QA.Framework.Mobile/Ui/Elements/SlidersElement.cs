using System;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;
//using Wonga.QA.Framework.UI.UiElements.Pages;
//using Wonga.QA.Framework.UI.UiElements.Pages.Common;
//using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
//using Wonga.QA.Framework.UI.Mappings;
//using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Framework.Mobile.UI.Elements
{
    public class SlidersElement : BaseElement
    {
        private readonly IWebElement _form;
        private readonly IWebElement _amountSlider;
        private readonly IWebElement _durationSlider;
        private readonly IWebElement _loanAmount;
        private readonly IWebElement _loanDuration;
        private IWebElement _submit;
        private readonly IWebElement _totalAmount;
        private readonly IWebElement _totalFees;
        private readonly IWebElement _totalToRepay;
        private readonly IWebElement _repaymentDate;
        private readonly IWebElement _amountMinusButton;
        private readonly IWebElement _amountPlusButton;
        private readonly IWebElement _durationMinusButton;
        private readonly IWebElement _durationPlusButton;

        public SlidersElement(BasePageMobile page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.FormId));
            _amountSlider = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.AmountSlider));
            _durationSlider = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.DurationSlider));
            _loanAmount = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.LoanAmount));
            _loanDuration = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.LoanDuration));
            _amountMinusButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.AmountMinusButton));
            _amountPlusButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.AmountPlusButton));
            _durationMinusButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.DurationMinusButton));
            _durationPlusButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.DurationPlusButton));
            _totalAmount = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.TotalAmount));
            _totalFees = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.TotalFees));
            _totalToRepay = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.TotalToRepay));
            _repaymentDate = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.RepaymentDate));
                    
            
        }

        public IWebElement LoanAmount
        {
            get { return _loanAmount; }
        }

        public IWebElement LoanDuration
        {
            get { return _loanDuration; }
        }

        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set
            {
                _loanAmount.EraseAll();
                _loanAmount.SendKeys(value);
                _loanAmount.LostFocus();
            }
        }
        public String HowLong
        {
            get { return _loanDuration.GetValue(); }
            set
            {
                _loanDuration.EraseAll();
                _loanDuration.SendKeys(value);
                _loanDuration.LostFocus();
            }
        }
        public int MoveAmountSlider //Moving by pixels NOT by cash value
        {
            set { Do.Until(() => _amountSlider.DragAndDropToOffset(value, 0)); }
        }
        public int MoveDurationSlider //Moving by pixels NOT by cash value
        {
            set { Do.Until(() => _durationSlider.DragAndDropToOffset(value, 0)); }
        }
        public String GetTotalAmount
        {
            get { return _totalAmount.Text; }
        }
        public String GetTotalFees
        {
            get { return _totalFees.Text; }
        }
        public String GetTotalToRepay
        {
            get { return _totalToRepay.Text; }
        }
        public String GetRepaymentDate
        {
            get { return _repaymentDate.Text; }
        }
        public void ClickAmountMinusButton()
        {
            _amountMinusButton.Click();
        }
        public void ClickAmountPlusButton()
        {
            _amountPlusButton.Click();
        }
        public void ClickDurationMinusButton()
        {
            _durationMinusButton.Click();
        }
        public void ClickDurationPlusButton()
        {
            _durationPlusButton.Click();
        }


        public IApplyPage Apply()
        {
            _submit = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.SubmitButton));
            _submit.Click();
            if (Config.AUT == AUT.Wb)
                //return new EligibilityQuestionsPage(Page.Client); //migrationFixNeeded
                return null;//migrationFixNeeded
            if (Config.AUT == AUT.Uk || Config.AUT == AUT.Za || Config.AUT == AUT.Ca || Config.AUT == AUT.Pl)
                //return new PersonalDetailsPage(Page.Client);
                return null; //migrationFixNeeded
            return null;
        }

        public IApplyPage ApplyLn()
        {
            _submit = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.SubmitButton));
            _submit.Click();
            switch (Config.AUT)
            {

                case AUT.Wb:
                case AUT.Za:
                case AUT.Uk:
                case AUT.Ca:
                    //return new ApplyPage(Page.Client); //migrationFixNeeded
                    return null; //migrationFixNeeded
                default:
                    throw new NotImplementedException();


            }
        }
        public bool IsSubmitButtonPresent()
        {
            try
            {
                _submit = _form.FindElement(By.CssSelector(UiMapMobile.Get.SlidersElement.SubmitButton));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
