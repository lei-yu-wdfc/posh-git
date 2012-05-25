using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Elements
{
    public class PrepaidBalanceBlock : BaseElement
    {
        private readonly IWebElement _balanceBlock;

        public PrepaidBalanceBlock(BasePage page) : base(page)
        {
            _balanceBlock = Page.Client.Driver.FindElement(By.CssSelector(UiMap.Get.PrepaidCardPage.BalanceBlock));
            Assert.IsTrue(_balanceBlock.Text.Contains(ContentMap.Get.PrepaidBalanceBlock.CurrentBalance));
        }

        public void DisplayForPremiumCard()
        {
            Assert.IsTrue(_balanceBlock.Text.Contains(ContentMap.Get.PrepaidBalanceBlock.MoneyOut));
            Assert.IsTrue(_balanceBlock.Text.Contains(ContentMap.Get.PrepaidBalanceBlock.MoneyOut));
            Assert.IsTrue(_balanceBlock.Text.Contains(ContentMap.Get.PrepaidBalanceBlock.CashbackEarned));
        }
    }
}
