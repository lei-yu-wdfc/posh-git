using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.Ui
{
    public class SeleniumTests : UiTest
    {
        [Test,AUT(AUT.Wb)]
        public void WbDeclinedLoan()
        {
            var processingPage = WbL0Path();
            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Za)]
        public void ZaDeclinedLoan()
        {
            var processingPage = ZaL0Path();
            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
        }
    }

}
