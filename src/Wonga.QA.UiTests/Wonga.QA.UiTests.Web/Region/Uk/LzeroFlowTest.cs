using System;
using System.Collections.Generic;
using System.Globalization;
using MbUnit.Framework;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.Self), AUT(AUT.Uk), Category(TestCategories.CoreTest)]
    public class LzeroFlowTest:UiTest
    {
        [Test, JIRA("UK-1624"), MultipleAsserts]
        public void stepTestAccount()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;
            var actualUrl = Client.Driver.Url.ToString();
            var accountPageUrl = Config.Ui.Home + "account";
            Client.Driver.Navigate().GoToUrl(Config.Ui.Home + "card");
            var cardAttemptUrl = Client.Driver.Url.ToString();
            Assert.AreEqual(accountPageUrl, cardAttemptUrl);
            Client.Driver.Navigate().Back();
            var backAttemptUrl = Client.Driver.Url.ToString();
            Assert.AreEqual(accountPageUrl, backAttemptUrl);
        }
    }
}