using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Wb.Journey
{
    [Parallelizable(TestScope.All), AUT(AUT.Wb)]
    public class L0LoanWb : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

        [Test]
        public void WbAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName(MiddleNameMask)
                .WithAddresPeriod("More than 4 years");
            var homePage = journey.Teleport<HomePage>() as HomePage;
        }

        [Test]
        public void WbAcceptedLoanAddAdditionalDirector()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName(MiddleNameMask)
                 .WithAddresPeriod("2 to 3 years")
                 .WithAdditionalDirrector();
            var homePage = journey.Teleport<HomePage>() as HomePage;
        }

        [Test]
        public void WbAcceptedLoanUpdateLoanDurationOnApplyTermsPage()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName(MiddleNameMask)
                .WithAddresPeriod("3 to 4 years");
            var applyTerms = journey.Teleport<ApplyTermsPage>() as ApplyTermsPage;
            journey.UpdateLoanDuration();
            var homePage = journey.Teleport<HomePage>() as HomePage;

        }

        [Test]
        public void WbAcceptedLoanAddressLessThan2Years()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName(MiddleNameMask)
                .WithAddresPeriod("Between 4 months and 2 years");
            var homePage = journey.Teleport<HomePage>() as HomePage;

        }

        [Test]
        public void WbDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithAddresPeriod("More than 4 years")
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
        }

    }
}
