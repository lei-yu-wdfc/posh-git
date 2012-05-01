using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Test, AUT(AUT.Uk), JIRA("PP-1")]
        public void DisplayPrepaidCardSubnavForEligibleCustomer()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            summaryPage.IsPrepaidCardButtonExist();
        }

        [Test,AUT(AUT.Uk),JIRA("PP-3")]
        public void DisplayLastRegisteredDetailsForEligibleCustomer()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            summaryPage.Navigation.MyPrepaidCardButtonClick();
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomer.Id);
        }
    }
}
