using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
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
            var processingPage = ZaL0Path("test:EmployedMask");

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();

        }

        //[Test, AUT(AUT.Ca)]
        public void CaAcceptedLoan()
        {
            var processingPage = CaL0Path("test:EmployedMask");

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignAgreementConfirm();
            acceptedPage.SignDirectDebitConfirm();
            var dealDone = acceptedPage.Submit();

        }

        [Test, AUT(AUT.Wb)]
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
