using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.Mappings;

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

            Assert.IsFalse(myPaymentDetailsPage.IsAddBankAccountButtonExists());


        }

    }
}
