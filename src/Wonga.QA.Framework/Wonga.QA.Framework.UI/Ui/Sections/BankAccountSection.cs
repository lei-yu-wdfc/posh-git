﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Sections
{
    public class BankAccountSection : BaseSection
    {
        private readonly IWebElement _bankName;
        private readonly IWebElement _sortCodePart1;
        private readonly IWebElement _sortCodePart2;
        private readonly IWebElement _sortCodePart3;
        private readonly IWebElement _accountNumber;
        private readonly IWebElement _bankPeriod;
        private readonly IWebElement _bankAccountType;
        private readonly IWebElement _institutionNumber;
        private readonly IWebElement _branchNumber;

        public String BankName { set { _bankName.SelectOption(value); } }
        public String AccountNumber { set { _accountNumber.SendValue(value); } }
        public String BankPeriod { set { _bankPeriod.SelectOption(value); } }
        public String BankAccountType { set { _bankAccountType.SelectOption(value); } }
        public String InstitutionNumber{set{_institutionNumber.SendValue(value);}}
        public String BranchNumber{set{_branchNumber.SendValue(value);}}

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

        public BankAccountSection(BasePage page) : base(UiMap.Get.BankAccountSection.Fieldset, page)
        {
            _bankName = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.BankName));
            _accountNumber = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.AccountNumber));
            _bankPeriod = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.BankPeriod));
            switch(Config.AUT)
            {
                case(AUT.Wb):
                case(AUT.Uk):
                    _sortCodePart1 = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.SortCodePart1));
                    _sortCodePart2 = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.SortCodePart2));
                    _sortCodePart3 = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.SortCodePart3));
                    break;
                case(AUT.Za):
                    _bankAccountType = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.BankAccountType));
                    break;
                case (AUT.Ca):
                    _institutionNumber = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.InstitutionNumber));
                    _branchNumber = Section.FindElement(By.CssSelector(UiMap.Get.BankAccountSection.BranchNumber));
                    break;

            }
            
        }
    }
}
