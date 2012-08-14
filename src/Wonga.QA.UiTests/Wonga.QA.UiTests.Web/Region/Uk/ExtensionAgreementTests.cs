﻿using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    class ExtensionAgreementTest : UiTest
    {
        [Test, JIRA("UKWEB-243", "UKWEB-294"), MultipleAsserts, Owner(Owner.StanDesyatnikov, Owner.OrizuNwokeji)]
        public void ExtensionAgreementPageTest()
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            const int loanAmount = 150;
            const int extensionDays = 7;
            const int loanTerm = 7;
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).WithLoanTerm(loanTerm).Build();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);
            requestPage.SetExtendDays(extensionDays.ToString("#"));

            // Set expected values
            var expectedRepaymentDate = DateTime.Today.AddDays(loanTerm).AddDays(extensionDays).ToString("dd/MM/yyyy");
            var expectedExtendedLoanTerm = application.LoanTerm + extensionDays;
            var expectedTotalToRepay = requestPage.TotalToRepay;
            var expectedLoanAmount = application.LoanAmount;
            var termDivisor = Convert.ToDouble(String.Format("{0:0.00000000}", 365d / Convert.ToDouble(expectedExtendedLoanTerm)));
            var loanDevisor = Convert.ToDouble(expectedTotalToRepay.Trim('£'))/Convert.ToDouble(expectedLoanAmount);
            var expectedRepresentativeApr = Math.Ceiling((Math.Pow(loanDevisor, termDivisor) - 1) * 100).ToString("#") + "%"; 
            
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();
            
            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            agreementPage.ClickExtensionSecciLink();
        
            /*Assert.IsTrue(agreementPage.secci.Text.Contains(ContentMap.Get.ExtensionAgreementPage.CreditInformation));

            Assert.Contains(agreementPage.secci.Text, expectedExtendedLoanTerm.ToString("#"));
            Assert.Contains(agreementPage.secci.Text, expectedRepaymentDate);
            Assert.Contains(agreementPage.secci.Text, expectedLoanAmount.ToString("#"));
            Assert.Contains(agreementPage.secci.Text, expectedTotalToRepay);
            Assert.Contains(agreementPage.secci.Text, expectedRepresentativeApr);*/


            string secciText = agreementPage.SecciPopupWindowContent();

            Assert.IsTrue(secciText.Contains(ContentMap.Get.ExtensionAgreementPage.CreditInformation));

            Assert.Contains(secciText, expectedExtendedLoanTerm.ToString("#"));
            Assert.Contains(secciText, expectedRepaymentDate);
            Assert.Contains(secciText, expectedLoanAmount.ToString("#"));
            Assert.Contains(secciText, expectedTotalToRepay);
            Assert.Contains(secciText, expectedRepresentativeApr);
        }

        [Test, JIRA("UKWEB-243", "UKWEB-294"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Row (2, 100, 1, 7)]
        public void ExtensionAgreementPageNDaysAfterLoanTest(int loanTerm, int loanAmount, int daysAfterLoan, int daysToExtend)
        {
            ExtensionAgreementPageNDaysAfterLoan(loanTerm, loanAmount, daysAfterLoan, daysToExtend);
        }

        #region Private Methods
        private void ExtensionAgreementPageNDaysAfterLoan(int loanTerm, int loanAmount, int daysAfterLoan, int daysToExtend)
        {
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            // Rewind application dates
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            Drive.Db.RewindApplicationDates(applicationEntity, riskApplication, TimeSpan.FromDays(daysAfterLoan));

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ChangePromiseDateButtonClick();
            var requestPage = new ExtensionRequestPage(this.Client);
            requestPage.SetExtendDays(daysToExtend.ToString("#"));

            // Set expected values
            var expectedExtendedLoanTerm = application.LoanTerm - daysAfterLoan + daysToExtend;
            var expectedRepaymentDate = Convert.ToDateTime(requestPage.RepaymentDate.Replace("st", "").Replace("nd", "").Replace("rd", "").Replace("th", "")).ToString("dd/MM/yyyy");
            var expectedTotalToRepay = requestPage.TotalToRepay;
            var expectedLoanAmount = application.LoanAmount;
            var termDivisor = Convert.ToDouble(String.Format("{0:0.00000000}", 365d / Convert.ToDouble(expectedExtendedLoanTerm)));
            var loanDevisor = Convert.ToDouble(expectedTotalToRepay.Trim('£')) / Convert.ToDouble(expectedLoanAmount);
            var expectedRepresentativeApr = Math.Ceiling((Math.Pow(loanDevisor, termDivisor) - 1) * 100).ToString("#") + "%";

            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var extensionProcessingPage = new ExtensionProcessingPage(this.Client);
            var agreementPage = extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;

            agreementPage.ClickExtensionSecciLink();

            string secciText = agreementPage.SecciPopupWindowContent();

            Assert.Contains(secciText, expectedExtendedLoanTerm.ToString("#"));
            Assert.Contains(secciText, expectedRepaymentDate);
            Assert.Contains(secciText, expectedLoanAmount.ToString("#"));
            Assert.Contains(secciText, expectedTotalToRepay);
            Assert.Contains(secciText, expectedRepresentativeApr);
        }
        #endregion
    }
}