using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class MySummaryScenarioTest : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-822"), Pending("Fails as cannot find webelement .summary-text .blue-text")]
        public void AccountOptionsCloudShown()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            //const decimal trustRating = 400.00M;

            var emailAddress1 = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(emailAddress1).Build();
            //var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(30).Build();
            ////var applicationId1 = application.Id;

            ////var accountId1 = customer.Id;

            var myAccountPage = loginPage.LoginAs(emailAddress1);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();
            mySummaryPage.CheckScenarioElementsExist();
        }
    }
}