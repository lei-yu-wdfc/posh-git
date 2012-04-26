using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Admin
{
    public class CashOutPage : AdminBasePage
    {
        private readonly IWebElement _searchButton;
        private ReadOnlyCollection<IWebElement> _sendPaymentCheckboxes;
        private ReadOnlyCollection<IWebElement> _cancelCheckboxes;
        private IWebElement _transactionTypeOptions;
        private IWebElement _filterOptions;
        private IWebElement _filterValue;
        private IWebElement _updateButton;
        

        public String SelectedTransactionType { set { _transactionTypeOptions.SelectOption(value); } }
        public String FilterBy { set { _filterOptions.SelectOption(value); } }
        public String FilterValue { set { _filterValue.SendValue(value); } }

        public CashOutPage(UiClient client)
            : base(client)
        {
            _transactionTypeOptions = Client.Driver.FindElement(By.CssSelector(Mappings.Ui.Get.CashOutPage.TransactionTypeOptions));
            _filterOptions = Client.Driver.FindElement(By.CssSelector(Mappings.Ui.Get.CashOutPage.FilterOptions));
            _filterValue = Client.Driver.FindElement(By.CssSelector(Mappings.Ui.Get.CashOutPage.FilterValue));
            _searchButton = Client.Driver.FindElement(By.CssSelector(Mappings.Ui.Get.CashOutPage.SearchButton));
        }

        public void Search()
        {
            _searchButton.Click();
        }

        public void GetSearchResults()
        {
            _sendPaymentCheckboxes = Client.Driver.FindElements(By.CssSelector(Mappings.Ui.Get.CashOutPage.SendPaymentCheckBox));
            _cancelCheckboxes = Client.Driver.FindElements(By.CssSelector(Mappings.Ui.Get.CashOutPage.CancelCheckBox));
            _updateButton = Client.Driver.FindElement(By.CssSelector(Mappings.Ui.Get.CashOutPage.UpdateButton));
        }

        public void MarkSendPayment(int i)
        {
            _sendPaymentCheckboxes[i].Click();
        }

        public void MarkCancel(int i)
        {
            _cancelCheckboxes[i].Click();
        }

        public void Update()
        {
            _updateButton.Click();
            Client.Driver.SwitchTo().Alert().Accept();
        }
    }
}