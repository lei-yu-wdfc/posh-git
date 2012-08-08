using System;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.Descendants), AUT(AUT.Uk), JIRA("UKWEB-8")]

    public class PromoCodeTests: UiTest
    {
        [Test, Pending("Test in development"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0_PromoCode_NewUser()
        {
            var homePage = Client.Home();
        }
    }
}
