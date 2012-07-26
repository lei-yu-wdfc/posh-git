using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class SalesForceTest : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("QA-220"), Pending("Problem with presents customers in SF")]
        public void CustomerInformationDisplayInSF()
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
		
    }
}
