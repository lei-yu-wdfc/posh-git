﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.UiElements.Sections;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class ApplyPage : BasePage, IApplyPage
    {
        public ApplicationSection ApplicationSection { get; set; }
        public ApplyPage(UiClient client)
            : base(client)
        {
            ApplicationSection = new ApplicationSection(this);
        }

        public BasePage Submit()
        {
            Client.Driver.FindElement(By.CssSelector(Ui.Get.ApplyPage.Submit)).Click();
            switch (Config.AUT)
            {
                case AUT.Za:
                case AUT.Ca:
                case AUT.Uk:
                    return new ProcessingPage(Client);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}