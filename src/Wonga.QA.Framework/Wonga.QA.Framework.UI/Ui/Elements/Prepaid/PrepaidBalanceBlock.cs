using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class PrepaidBalanceBlock : BaseElement
    {
        private readonly IWebElement _balanceBlock;
        private  IWebElement _currentBalance;
        private IWebElement _cashback;
        private IWebElement _moneyIn;
        private IWebElement _moneyOut;


        public PrepaidBalanceBlock(BasePage page) : base(page)
        {
            _balanceBlock = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.BalanceBlock));
            _currentBalance = _balanceBlock.FindElement(By.LinkText(Content.Get.PrepaidbalanceBlock.CurrentBalance));
        }

        public void DisplayForPremiumCard()
        {
            _cashback = _balanceBlock.FindElement(By.LinkText(Content.Get.PrepaidbalanceBlock.CashbackEarned));
            _moneyIn = _balanceBlock.FindElement(By.LinkText(Content.Get.PrepaidbalanceBlock.MoneyIn));
            _moneyOut = _balanceBlock.FindElement(By.LinkText(Content.Get.PrepaidbalanceBlock.MoneyOut));
        }
    }
}
