using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    public class RepaymentTest : UiTest
    {
        [Test, AUT(AUT.Za)]
        public void ZaManualNaedoRepayment()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var repayPage = mySummaryPage.RepayClick();

            repayPage.PayByDebitOrderButtonButtonClick();
        }

        [Test, AUT(AUT.Za)]
        public void ZaEasyPayRepayment()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var repayPage = mySummaryPage.RepayClick();
           
            // Open the "How to use easypay" modal popup and check the title is correct - ZA-2587:
            repayPage.HowToUseEasyPayLink.Click();

            // Wait until the popup opens:
            var popUp = Client.Driver.FindElement(By.CssSelector("div#fancybox-content"));
            Do.With.Message("Popup don't closed").Until(() => popUp.Displayed);

            var text = popUp.FindElement(By.Id("repay-your-wonga.com-loan-with-easypay")).Text;

            Assert.AreEqual(text, "Repay your wonga.com loan with EasyPay");

            // Close the popup:
            Client.Driver.FindElement(By.CssSelector("#fancybox-close")).Click();

            var expectedeasypayno = repayPage.EasypayNumber;
            var popUpPrintPage = repayPage.EasyPayPrintButtonClick();
            var actualString = Do.With.Message("There is no sought-for element on a page").Until(() => popUpPrintPage.FindElement(By.CssSelector(UiMap.Get.EasypaymentNumberPrintPage.YourEasyPayNumber)).Text);
            Assert.IsTrue(actualString.StartsWith(">>>>>> "));
            Assert.IsTrue(actualString.EndsWith(expectedeasypayno));
        }

        [Test, AUT(AUT.Za)]
        public void RepayByDebitCardTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var repayPage = mySummaryPage.RepayClick();

            repayPage.RepayByDebitCard();
            
        }
    }
}
