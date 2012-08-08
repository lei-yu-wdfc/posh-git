using System;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Wb.MyAccounts
{
    [Parallelizable(TestScope.All)]
    public class HelpTest : UiTest
    {
        [Test, AUT(AUT.Wb), JIRA("QA-291")]
        public void ContactPageMustContainOnlyDetailsForWongaBuisness()
        {
            var page = Client.Home();
            page.Help.HelpTriggerClick();
            page.Help.ContactUsClick();
            List<string> wbEmails = page.Contact.GetLinksTextFromPopup();
            foreach (string email in wbEmails)
            {
                Assert.IsTrue(email.Contains("@wongabusiness.com"));
            }
        }
    }
}
