using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.UiTests.Web.Region.Wb.MyAccounts
{
    [Parallelizable(TestScope.All), AUT(AUT.Wb)]
    public class WbMyAccountTests : UiTest
    {
        [Test, JIRA("QA-250")]
        public void WbFrontendMyAccountPageLoadsCorrectly()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Organisation organisation = OrganisationBuilder.New(customer).Build();
            Application application = ApplicationBuilder
                .New(customer, organisation)
                .Build();

            var mySummaryPage = loginPage.LoginAs(email);
        }
    }
}
