using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Framework;

namespace Wonga.QA.Framework.UI.Elements
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
        private readonly IWebElement _maxAvailableCredit;
        private readonly IWebElement _termsOfLoan;
        public SlidersElement(BasePage page)
            : base(page)
        {
            _form = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.SlidersElement.FormId));
            _amountSlider = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.AmountSlider));
            _durationSlider = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.DurationSlider));
            _loanAmount = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.LoanAmount));
            _loanDuration = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.LoanDuration));
            _amountMinusButton = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.AmountMinusButton));
            _amountPlusButton = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.AmountPlusButton));
            _durationMinusButton = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.DurationMinusButton));
            _durationPlusButton = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.DurationPlusButton));
            switch (Config.AUT)
            {
                case (AUT.Wb):
                    var riskBasedPricingEnabled = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.Wb.RiskBasedPricingEnabled").Value;
                    bool isRiskBasedPricingEnabled = Boolean.Parse(riskBasedPricingEnabled);
                    if (isRiskBasedPricingEnabled)
                    {
                       break;
                    }
                    _totalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalAmount));
                    _totalFees = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalFees));
                    _totalToRepay = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalToRepay));
                    _termsOfLoan = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TermsOfLoan));
                        break;
                case (AUT.Ca):
                case (AUT.Za):
                    _totalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalAmount));
                    _totalFees = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalFees));
                    _totalToRepay = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalToRepay));
                    _repaymentDate = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.RepaymentDate));
                    break;
                case (AUT.Uk):
                    _totalAmount = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.TotalAmount));
                    _maxAvailableCredit = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.MaxAvailableCredit));
                    break;
            }
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

        public String GetsTermOfLoan
        {
            get { return _termsOfLoan.Text; }
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
            _submit = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.SubmitButton));
            _submit.Click();
            if (Config.AUT == AUT.Wb)
                return new EligibilityQuestionsPage(Page.Client);
            if (Config.AUT == AUT.Uk || Config.AUT == AUT.Za || Config.AUT == AUT.Ca || Config.AUT == AUT.Pl)
                return new PersonalDetailsPage(Page.Client);
            return null;
        }

        public IApplyPage ApplyLn()
        {
            _submit = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.SubmitButton));
            _submit.Click();
            switch (Config.AUT)
            {

                case AUT.Wb:
                case AUT.Za:
                case AUT.Uk:
                case AUT.Ca:
                    return new ApplyPage(Page.Client);
                default:
                    throw new NotImplementedException();


            }
        }
        public bool IsSubmitButtonPresent()
        {
            try
            {
                _submit = _form.FindElement(By.CssSelector(UiMap.Get.SlidersElement.SubmitButton));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public String MaxAvailableCredit()
        {
            return _maxAvailableCredit.Text.Replace("max £", "");
        }

    }
}
