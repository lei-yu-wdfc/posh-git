using System;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.Descendants), AUT(AUT.Uk), JIRA("UKWEB-8")]

    public class PromoCodeTests: UiTest
    {
        private string _emailL0;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _emailL0 = Get.RandomEmail();
            Console.WriteLine("Email={0}", _emailL0);

            Customer customerL0 = CustomerBuilder.New().WithEmailAddress(_emailL0).Build();
            ApplicationBuilder.New(customerL0).WithLoanAmount(100).Build();
        }

        [Test, Pending("Test in development"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0_NewUser_SubmitInvalidPromoCode()
        {
            Client.Driver.Manage().Window.Maximize();
            var homePage = Client.Home();

            //Submit invalid promo code
            homePage.PromoCode = "asdf";
            homePage.ClickGoButton();

            //Ensure correct error message is displayed when invalid promo code is submitted
            //TODO - Change 'expected value' to "Sorry, invalid promo code" when fixed
            Assert.AreEqual("Sorry, code is invalid", homePage.GetPromoCodeInValidMessage());
            Console.WriteLine(homePage.GetPromoCodeInValidMessage());
        }

        [Test, Pending("Test in development"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0_NewUser_SubmitExpiredPromoCode()
        {
            Client.Driver.Manage().Window.Maximize();
            var homePage = Client.Home();

            //Submit invalid promo code
            homePage.PromoCode = "asdf";
            homePage.ClickGoButton();

            //Ensure correct error message is displayed when invalid promo code is submitted
            //TODO - Change 'expected value' to "Sorry, invalid promo code" when fixed
            Assert.AreEqual("Sorry, code is invalid", homePage.GetPromoCodeInValidMessage());
            Console.WriteLine(homePage.GetPromoCodeInValidMessage());
        }

        [Test, Pending("Test in development"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0_NewUser_SubmitOverUsedPromoCode()
        {
            Client.Driver.Manage().Window.Maximize();
            var homePage = Client.Home();

            //Submit invalid promo code
            homePage.PromoCode = "asdf";
            homePage.ClickGoButton();

            //Ensure correct error message is displayed when invalid promo code is submitted
            //TODO - Change 'expected value' to "Sorry, invalid promo code" when fixed
            Assert.AreEqual("Sorry, code is invalid", homePage.GetPromoCodeInValidMessage());
            Console.WriteLine(homePage.GetPromoCodeInValidMessage());
        }

        [Test, Pending("Test in development"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0_NewUser_SubmitValidPromoCode()
        {
            Client.Driver.Manage().Window.Maximize();
            var homePage = Client.Home();

            //Submit invalid promo code
            homePage.PromoCode = "asdf";
            homePage.ClickGoButton();

            //Ensure correct message is displayed when valid promo code is submitted
            Assert.AreEqual("Promo code accepted", homePage.GetPromoCodeInValidMessage());
            Console.WriteLine(homePage.GetPromoCodeInValidMessage());

            //Ensure Slider loan summary shows correct values
            Assert.AreEqual("Borrowing £265 + Interest & fees* £61.53 = Total to repay £326.5" +
                            " Your promo code has been accepted and a discount of £5.50 applied to your total to repay",
                            homePage.GetSliderLoanSummaryText());
        }
    }
}
