using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssessmentIncomeTest : UiTest
    {
        public Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            return customer;
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-720"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Category(TestCategories.SmokeTest)]
        public void NextClickWithValidDataAndCheckDataAfterGoBack()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faincomepage = journeyLn.FillAndStop().WithEmployer(Get.GetEmployerName()).Teleport<FAIncomePage>() as FAIncomePage;

            faincomepage.NextClick();

          
        }
    }
}
