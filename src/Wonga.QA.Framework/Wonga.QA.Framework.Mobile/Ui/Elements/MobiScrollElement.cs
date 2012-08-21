using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Mobile.Mappings.Ui;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Ui.Elements
{
    public class MobiScrollElement
    {
        public IWebElement DatePicker;
        public IWebElement Done;
        public IWebElement Cancel;


        public MobiScrollElement(MobileUiClient client)
        {
            DatePicker = client.Driver.FindElement(By.CssSelector(UiMapMobile.Get.MobiScrollElement.DatePicker));
            Done = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.MobiScrollElement.Done));
            Cancel = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.MobiScrollElement.Cancel));
        }

        public void SelectDefaultDate()
        {
            //default dob is set to 01/Jan/1970
            Done.Click();
        }
        
    }

    public class MonthYearMobiScrollElement : MobiScrollElement
    {
        private readonly IWebElement _increaseMonth;
        private readonly IWebElement _increaseYear;
        private readonly IWebElement _decreaseMonth;
        private readonly IWebElement _decreaseYear;
        
        public MonthYearMobiScrollElement(MobileUiClient client)
            : base(client)
        {
            _increaseMonth = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.MonthYearMobiScrollElement.IncreaseMonth));
            _increaseYear = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.MonthYearMobiScrollElement.IncreaseYear));
            _decreaseMonth = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.MonthYearMobiScrollElement.DecreaseMonth));
            _decreaseYear = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.MonthYearMobiScrollElement.DecreaseYear));
        }

        public void SelectExpiryDate()
        {
            for(int i=0; i < 2; i++)
            {
                _increaseYear.Click();
            }
            Done.Click();            
        }
    }

    public class DayMonthYearMobiScrollElement : MobiScrollElement
    {
        private readonly IWebElement _increaseDay;
        private readonly IWebElement _increaseMonth;
        private readonly IWebElement _increaseYear;
        private readonly IWebElement _decreaseDay;
        private readonly IWebElement _decreaseMonth;
        private readonly IWebElement _decreaseYear;

        public DayMonthYearMobiScrollElement(MobileUiClient client)
            : base(client)
        {
            _increaseDay = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.DayMonthYearMobiScrollElement.IncreaseDay));
            _increaseMonth = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.DayMonthYearMobiScrollElement.IncreaseMonth));
            _increaseYear = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.DayMonthYearMobiScrollElement.IncreaseYear));
            _decreaseDay = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.DayMonthYearMobiScrollElement.DecreaseDay));
            _decreaseMonth = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.DayMonthYearMobiScrollElement.DecreaseMonth));
            _decreaseYear = DatePicker.FindElement(By.CssSelector(UiMapMobile.Get.DayMonthYearMobiScrollElement.DecreaseYear));
        }

        public void SelectNextPayDate()
        {
            if (DateTime.Now.Month.Equals(12)) //if the date time month is December, increase year before increasing month to January
            {
                _increaseYear.Click();
                _increaseMonth.Click();
            }
            else
            {
                _increaseMonth.Click();
            }
            Done.Click();
        }
    }
}
