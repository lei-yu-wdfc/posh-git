using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Prepaid
{
    class PrepaidAccountTests : UiTest
    {
        private Customer _eligibleCustomer = null;

        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id, true);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-1"), Pending("Times out with the exception: No matching table found, or insufficient permissions.")]
        public void DisplayPrepaidCardSubnavForEligibleCustomer()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            summaryPage.IsPrepaidCardButtonExist();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-3"), Pending("Times out with the exception: No matching table found, or insufficient permissions.")]
        public void DisplayLastRegisteredDetailsForEligibleCustomer()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage.ApplyCardButtonClick();

            var dictionary = CustomerOperations.GetFullCustomerInfo(_eligibleCustomer.Id);
            var pageSoruce = prepaidPage.Client.Source();
               
            Assert.IsTrue(pageSoruce.Contains(dictionary[CustomerOperations.CUSTOMER_FULL_NAME]));
            Assert.IsTrue(pageSoruce.Contains(dictionary[CustomerOperations.CUSTOMER_FULL_ADDRESS]));
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomer.Id);
        }
    }
}
