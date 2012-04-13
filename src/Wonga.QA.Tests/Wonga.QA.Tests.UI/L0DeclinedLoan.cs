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
        [Test, AUT(AUT.Wb), Pending("Broken, waiting for FE to stabilize changes..")]
        public void WbDeclinedLoan()
        {
            //var processingPage = WbL0Path();
            //var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
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

        [Test, AUT(AUT.Ca), Pending("CA WIP,RC FE seems broken - postponing the push of the selenium tests")]
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
    }

}
