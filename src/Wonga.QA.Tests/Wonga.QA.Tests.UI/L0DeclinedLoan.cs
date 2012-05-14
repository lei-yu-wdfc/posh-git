using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.Ui
{
    public class L0DeclinedLoan : UiTest
    {
        [Test, AUT(AUT.Wb)]
        public void WbDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
            journey.ApplyForLoan(5500, 30)
                .AnswerEligibilityQuestions()
                .FillPersonalDetails()
                .FillAddressDetails("More than 4 years")
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .EnterBusinessDetails()
                .DeclineAddAdditionalDirector()
                .EnterBusinessBankAccountDetails()
                .EnterBusinessDebitCardDetails()
                .WaitForDeclinedPage();
        }

        [Test, AUT(AUT.Za)]
        public void ZaDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails()
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;

            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Ca)]
        public void CaDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails()
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;
            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Za), JIRA("QA-278"), Category(TestCategories.Smoke), Pending("ZA-2302")]
        public void ZaDeclinedPageContainsHeaderLinks()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails()
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;

            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            declinedPage.LookForHeaderLinks();
        }
    }

}
