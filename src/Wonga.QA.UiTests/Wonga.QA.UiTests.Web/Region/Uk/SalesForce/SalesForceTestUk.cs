using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Za.SalesForce
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    class SalesForceTestUk : UiTest
    {
        [Test, JIRA("QA-220")]
        public void UkCustomerInformationDisplayInSF()
        {
            string email = Get.RandomEmail();

            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();

            Thread.Sleep(30000);

            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.Salesforce.Username, Config.Salesforce.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);

            Thread.Sleep(2000);

            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());

            salesForceSearchResultPage.GoToCustomerDetailsPage();
        }

        [Test, JIRA("QA-220")]
        public void UkCustomerLoanStatusDisplayedInSF()
        {
            string email = Get.RandomEmail();

            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.PutIntoArrears(5);
            Thread.Sleep(30000);

            var salesForceStartPage = Client.SalesForceStart();
            var salesForceHome = salesForceStartPage.LoginAs(Config.Salesforce.Username, Config.Salesforce.Password);
            var salesForceSearchResultPage = salesForceHome.FindCustomerByMail(email);

            Thread.Sleep(2000);

            Assert.IsTrue(salesForceSearchResultPage.IsCustomerFind());
            var salesForceCustomerDetailsPage =  salesForceSearchResultPage.GoToCustomerDetailsPage();
            Assert.AreEqual(salesForceCustomerDetailsPage.LoanStatus, "New");

        }
    }
}
