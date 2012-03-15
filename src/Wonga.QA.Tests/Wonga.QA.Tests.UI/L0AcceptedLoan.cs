using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    public class L0AcceptedLoan : UiTest
    {
        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoan()
        {
            var journey = new Journey(Client.Home());
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails("test:EmployedMask")
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();

        }

       [Test, AUT(AUT.Ca), Pending("CA WIP,RC FE seems broken - postponing the push of the selenium tests")]
        public void CaAcceptedLoan()
        {
            var journey = new Journey(Client.Home());
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails("test:EmployedMask")
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignConfirmCA(DateTime.Now.ToString("d MMM yyyy"), _firstName, _lastName);
            var dealDone = acceptedPage.Submit();
        }

        [Test, AUT(AUT.Wb), Pending("Broken, waiting for FE to stabilize changes..")]
        public void WbAcceptedLoan()
        {
            var processingPage = WbL0Path("TESTNoCheck");
            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
            var dealDonePage = acceptedPage.Submit() as DealDonePage;
        }
    }
}
