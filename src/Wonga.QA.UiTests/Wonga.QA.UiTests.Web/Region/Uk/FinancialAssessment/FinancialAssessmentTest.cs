using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssessmentTest : UiTest
    {
        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), MultipleAsserts, Pending("Financial Assessment")]
        public void LoginAsCustomerWithDifferentLoanTime()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            loginPage.LoginAs(email);

            var financialassessmentpage = Client.FinancialAssessment();
            Assert.IsTrue(financialassessmentpage.IsNotAvailablePage(), "Login without loan");

            Application application = ApplicationBuilder.New(customer).WithLoanTerm(7).Build();
            financialassessmentpage = Client.FinancialAssessment();
            Assert.IsFalse(financialassessmentpage.IsNotAvailablePage(), "Login with 7 days loan");

            application.UpdateNextDueDate(8);
            financialassessmentpage = Client.FinancialAssessment();
            Assert.IsTrue(financialassessmentpage.IsNotAvailablePage(), "Login with 8 days loan");

            application.UpdateNextDueDate(-3);
            financialassessmentpage = Client.FinancialAssessment();
            Assert.IsFalse(financialassessmentpage.IsNotAvailablePage(), "Login with 3 days loan in arrears");

            application.UpdateNextDueDate(-4);
            financialassessmentpage = Client.FinancialAssessment();
            Assert.IsTrue(financialassessmentpage.IsNotAvailablePage(), "Login with 4 days loan in arrears");
        }

        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), MultipleAsserts, Pending("Financial Assessment")]
        public void PrepopulatedNameAndAssesmentEmailCheck()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            loginPage.LoginAs(email);

            var financialassessmentpage = Client.FinancialAssessment();

            Assert.Contains(financialassessmentpage.GetPrepopulatedName(), customer.GetCustomerForename(), "Prepopulated name check");
            Assert.AreEqual(ContentMap.Get.FinancialAssessmentPage.AssesmentEmail, financialassessmentpage.GetAssesmentsEmail(), "Prepopulated email check");
        }

        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), Pending("Financial Assessment")]
        public void NavigateByClickGetStarted()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            loginPage.LoginAs(email);

            var financialassessmentpage = Client.FinancialAssessment();
            var faaboutyoupage = financialassessmentpage.GetStartedClick() as FAAboutYouPage;
            Assert.Contains(faaboutyoupage.Url, "/about-you");
        }
    }
}
