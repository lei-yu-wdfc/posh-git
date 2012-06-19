using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class L0DeclinedLoan : UiTest
    {
        [Test, AUT(AUT.Wb)]
        public void WbDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithAddresPeriod("More than 4 years")
                .WithDeclineDecision();
            var declinePage = journey.Teleport<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Za, AUT.Ca)]
        public void DeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Za), JIRA("QA-278"), Pending("ZA-2302")]
        public void ZaDeclinedPageContainsHeaderLinks()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
            declinedPage.LookForHeaderLinks();
        }
    }
}
