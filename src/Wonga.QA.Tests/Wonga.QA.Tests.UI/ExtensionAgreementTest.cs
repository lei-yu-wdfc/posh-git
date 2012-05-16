using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Gallio.Framework.Assertions;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Sections;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.Mappings;

using System.ComponentModel;

namespace Wonga.QA.Tests.Ui
{
    class ExtensionAgreementTest : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-971")]
        public void ExtensionAgreementPageTest()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var extensionDays = 7;
            var sExtensionDays = extensionDays.ToString();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(extensionDays).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);

            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);
            requestPage.SetExtendDays(sExtensionDays);
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            agreementPage.ClickExtensionSecciLink();
        
            var api = new ApiDriver();
            var response = api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery { ApplicationId = application.Id });

            // Set expected values
            var oweToday = decimal.Parse(response.Values["TotalAmountDueToday"].Single());
            var sOweToday = String.Format("£{0}", oweToday.ToString("#.00"));
            var totalRepayToday = decimal.Parse(response.Values["ExtensionPartPaymentAmount"].Single());
            var sTotalRepayToday = String.Format("Total £{0}", totalRepayToday.ToString("#.00"));
            var newCreditAmount = decimal.Parse(response.Values["CurrentPrincipleAmount"].Single());
            var sNewCreditAmount = String.Format("£{0}", newCreditAmount.ToString("#.00"));
            var expectedRepaymentDate = DateTime.Parse(response.Values["ExtensionDate"].ToArray()[extensionDays - 1]).Date.ToString("dd/MM/yyyy");
            var totalRepaymentStr = System.String.Format("You will pay to us {0} in one payment before 5pm on the {1} (\"the promise date\").", sOweToday, expectedRepaymentDate);

            Assert.IsTrue(agreementPage.secci.Text.Contains("PRE-CONTRACT CREDIT INFORMATION"));
            Assert.IsTrue(agreementPage.secciPrint.Text.Contains("Print this page"));
            Assert.IsTrue(agreementPage.secciHeader.Text.Contains("Please read this document carefully and print it off for your records"));
 
            //Assert.IsTrue(agreementPage.secci.Text.Contains(sOweToday));
            //Assert.IsTrue(agreementPage.secci.Text.Contains(sTotalRepayToday)); //Partial payment
            //Assert.IsTrue(agreementPage.secci.Text.Contains(totalRepaymentStr)); //Repayment phrase

        }
    }
}