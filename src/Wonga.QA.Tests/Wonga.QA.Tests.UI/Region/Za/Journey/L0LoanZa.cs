using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class L0LoanZa : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home()); 
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails(employerNameMask: Get.EnumToString(RiskMask.TESTEmployedMask))
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();
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

        [Test, AUT(AUT.Za), JIRA("QA-278"), Pending("ZA-2302")]
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
