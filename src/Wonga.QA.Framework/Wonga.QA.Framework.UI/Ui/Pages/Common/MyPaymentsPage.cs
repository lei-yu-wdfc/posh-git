using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class MyPaymentsPage : BasePage
    {
        public MyPaymentsPage(UiClient client)
            : base(client)
        {
        }
        public bool IsAddBankAccountButtonExists()
        {
            try
            {
                switch (Config.AUT)
                {
                    case AUT.Za:
                    case AUT.Ca:  //TODO find out what xpath for button on Ca
                        var addBankAccountButton =
                            Client.Driver.FindElement(By.XPath(Ui.Get.MyPaymentsPage.AddBankAccountButton));
                        break;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return true;
        }
    }
}