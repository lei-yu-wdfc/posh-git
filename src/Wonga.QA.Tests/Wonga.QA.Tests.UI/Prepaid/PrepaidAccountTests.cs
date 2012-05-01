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
        [Test, AUT(AUT.Uk), JIRA("PP-1")]
        public void DisplayPrepaidCardSubnavForEligibleCustomer()
        {
            Customer eligibleCustomer = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(eligibleCustomer.Id, true);

            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(eligibleCustomer.GetEmail());
            summaryPage.IsPrepaidCardButtonExist();
        }
    }
}
