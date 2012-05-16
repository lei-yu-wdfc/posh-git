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
        [Test, AUT(AUT.Uk), JIRA("UK-971"), Pending("Fails due to bug UK-2114")]
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

            // Set expected values
            var expectedRepaymentDate = Convert.ToDateTime(requestPage.RepaymentDate.Replace("st", "").Replace("nd", "").Replace("rd","").Replace("th", "")).ToString("dd/MM/yyyy");
            var expectedExtendedLoanTerm = application.LoanTerm + extensionDays;
            var expectedTotalToRepay = requestPage.TotalToRepay;
            var expectedLoanAmount = application.LoanAmount;
            var expectedRepresentativeAPR = "3784%";
            
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            agreementPage.ClickExtensionSecciLink();
        
            Assert.IsTrue(agreementPage.secci.Text.Contains("PRE-CONTRACT CREDIT INFORMATION"));
            Assert.IsTrue(agreementPage.secciPrint.Text.Contains("Print this page"));
            Assert.IsTrue(agreementPage.secciHeader.Text.Contains("Please read this document carefully and print it off for your records"));
          
            Assert.Contains(agreementPage.secci.Text, expectedExtendedLoanTerm + " day");
            Assert.Contains(agreementPage.secci.Text, "before 5pm on the " + expectedRepaymentDate);
            Assert.Contains(agreementPage.secci.Text, "Total £" + expectedLoanAmount);
            Assert.Contains(agreementPage.secci.Text, "You will pay to us " + expectedTotalToRepay);
            Assert.Contains(agreementPage.secci.Text, "APR: " + expectedRepresentativeAPR);
        }
    }
}