using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    class MyAccountTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-218")]
        public void CustomerWithLiveLoanShouldNotBeAbleToAddBankAccount()
        {
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPaymentDetailsPage = mySummaryPage.Navigation.MyPaymentDetailsButtonClick();

            Assert.IsFalse(IsAddBankAccountButtonExists());


        }
        private bool IsAddBankAccountButtonExists()
        {
            try
            {
                switch (Config.AUT)
                {
                    case AUT.Za:
                    case AUT.Ca:  //TODO find out what xpath for button on Ca
                        var addBankAccountButton =
                            Client.Driver.FindElement(By.XPath("//tr[@class='accounts-add-bank']"));
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
