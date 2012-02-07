using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.IO;
using OpenQA.Selenium;

namespace Wonga.QA.Framework.UI.Pages.Wb
{
    public class BankAccountDetailsPage : BasePage
    {
        private readonly IWebElement _form;
        private readonly IWebElement _bankName;
        private readonly IWebElement _sortCodePart1;
        private readonly IWebElement _sortCodePart2;
        private readonly IWebElement _sortCodePart3;
        private readonly IWebElement _accountNumber;
        private readonly IWebElement _bankPeriod;
        private readonly IWebElement _next;

        public String BankName { set { _bankName.SelectOption(value); } }
        public String AccountNumber { set { _accountNumber.SendValue(value); } }
        public String BankPeriod { set { _bankPeriod.SelectOption(value); } }
        public String SortCode
        {
            set
            {
                var sortCode = value.Split('-');
                _sortCodePart1.SendValue(sortCode[0]);
                _sortCodePart2.SendValue(sortCode[1]);
                _sortCodePart3.SendValue(sortCode[2]);
            }
        }

        public BankAccountDetailsPage(UiClient client)
            : base(client)
        {
            _form = Content.FindElement(By.Id("lzero-bank-form"));

            _bankName = _form.FindElement(By.Name("bank_name"));
            _sortCodePart1 = _form.FindElement(By.Name("sort_code[part1]"));
            _sortCodePart2 = _form.FindElement(By.Name("sort_code[part2]"));
            _sortCodePart3 = _form.FindElement(By.Name("sort_code[part3]"));
            _accountNumber = _form.FindElement(By.Name("account_number"));
            _bankPeriod = _form.FindElement(By.Name("bank_period"));
            _next = _form.FindElement(By.Name("next"));
        }

        public DebitCardDetailsPage Next()
        {
            _next.Click();
            return new DebitCardDetailsPage(Client);
        }
    }
}
