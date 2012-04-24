using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Framework.UI.UiElements.Sections;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class PersonalDetailsPage : BasePage, IApplyPage
    {
        public YourNameSection YourName { get; set; }
        public EmploymentDetailsSection EmploymentDetails { get; set; }
        public YourDetailsSection YourDetails { get; set; }
        public ContactingYouSection ContactingYou { get; set; }
        public ProvinceSection ProvinceSection { get; set; }
        public Boolean PrivacyPolicy { set { _privacy.Toggle(value); } }
        public Object CanContact
        {
            set
            {
                if (value is bool)
                    _contact.Single().Toggle((Boolean)value);
                else
                    _contact.SelectLabel((String)value);
            }
        }
        public string MarriedInCommunityProperty
        {
            set
            {
                _marriedInCommunityProperty.SelectLabel(value);
            }
        }
        private readonly IWebElement _form;
        private readonly IWebElement _slidersForm;
        private readonly ReadOnlyCollection<IWebElement> _marriedInCommunityProperty;
        private readonly IWebElement _privacy;
        private readonly ReadOnlyCollection<IWebElement> _contact;
        private readonly IWebElement _next;
        private readonly IWebElement _totalToRepay;
        private readonly IWebElement _repaymentDate;
        private readonly IWebElement _totalAmount;
        private readonly IWebElement _totalFees;
        private IWebElement _amountSlider;
        private IWebElement _durationSlider;
        private IWebElement _loanAmount;
        private IWebElement _loanDuration;
        private IWebElement _amountMinusButton;
        private IWebElement _amountPlusButton;
        private IWebElement _durationMinusButton;
        private IWebElement _durationPlusButton;


        public PersonalDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.FormId));
            if (!Config.AUT.Equals(AUT.Wb))
            {
                //On WB you cannot edit your loan details on the Personal Details page
                _slidersForm = Content.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.SlidersFormId));
                _totalToRepay = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.TotalToRepay));
                _repaymentDate = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.RepaymentDate));
                _totalAmount = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.TotalAmount));
                _totalFees = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.TotalFees));
            }
            _privacy = _form.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.CheckPrivacyPolicy));
            _contact = _form.FindElements(By.CssSelector(Ui.Get.PersonalDetailsPage.CheckCanContact));
            _next = _form.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.NextButton));

            YourName = new YourNameSection(this);
            YourDetails = new YourDetailsSection(this);
            ContactingYou = new ContactingYouSection(this);

            switch (Config.AUT)
            {
                case (AUT.Ca):
                    EmploymentDetails = new EmploymentDetailsSection(this);
                    ProvinceSection = new ProvinceSection(this);
                    break;
                case (AUT.Uk):
                    EmploymentDetails = new EmploymentDetailsSection(this);
                    break;
                case (AUT.Za):
                    EmploymentDetails = new EmploymentDetailsSection(this);
                    _marriedInCommunityProperty =
                        _form.FindElements(By.CssSelector(Ui.Get.PersonalDetailsPage.MarriedInCommunityProperty));
                    break;
            }
        }

        public void ClickSliderToggler()
        {
            var sliderToggler = Client.Driver.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.SliderToggler));
            sliderToggler.Click();
            _amountSlider = _slidersForm.FindElement(By.CssSelector(Ui.Get.SlidersElement.AmountSlider));
            _durationSlider = _slidersForm.FindElement(By.CssSelector(Ui.Get.SlidersElement.DurationSlider));
            _loanAmount = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.LoanAmount));
            _loanDuration = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.LoanDuration));
            _amountMinusButton = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.AmountMinusButton));
            _amountPlusButton = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.AmountPlusButton));
            _durationMinusButton =
                _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.DurationMinusButton));
            _durationPlusButton = _slidersForm.FindElement(By.CssSelector(Ui.Get.PersonalDetailsPage.DurationPlusButton));

        }
        public int MoveAmountSlider
        {
            set { _amountSlider.DragAndDropToOffset(value, 0); }
        }
        public int MoveDurationSlider
        {
            set { _durationSlider.DragAndDropToOffset(value, 0); }
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
        public String HowMuch
        {
            get { return _loanAmount.GetValue(); }
            set { _loanAmount.SendValue(value); }
        }
        public String HowLong
        {
            get { return _loanDuration.GetValue(); }
            set { _loanDuration.SendValue(value); }
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

        public IApplyPage Submit()
        {
            _next.Click();
            return new AddressDetailsPage(Client);
        }
    }
}
