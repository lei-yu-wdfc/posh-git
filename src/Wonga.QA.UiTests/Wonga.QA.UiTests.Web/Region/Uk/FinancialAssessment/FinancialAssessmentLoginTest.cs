using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssessmentLoginTest : UiTest
    {
        [Test, AUT(AUT.Uk), Owner(Owner.SaveliyProkopenko), Category(TestCategories.SmokeTest), Pending("Financial Assessment")]
        public void LoginTest()
        {
            var faloginpage = Client.FinancialAssessmentLogin();
            string email = Get.RandomEmail();
            string password = "Password123";
            Customer customer = CustomerBuilder.New().WithPassword(password).WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();

            var financialassessmentpage = faloginpage.LoginAs(email, password) as FinancialAssessmentPage;

            Assert.EndsWith(financialassessmentpage.Url, "financial-assessment");
        }
    }
}
