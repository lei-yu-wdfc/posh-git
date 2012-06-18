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
    public class L0LoanCa : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

       [Test, AUT(AUT.Ca), SmokeTest]
        public void CaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)); 
            var mySummary = journey.ApplyForLoan()
                                 .FillPersonalDetails()
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .WaitForAcceptedPage()
                                 .FillAcceptedPage()
                                 .GoToMySummaryPage();
        }

       [Test, AUT(AUT.Ca)]
       public void CaDeclinedLoan()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home());
           var processingPage = journey.ApplyForLoan()
                                    .FillPersonalDetails()
                                    .FillAddressDetails()
                                    .FillAccountDetails()
                                    .FillBankDetails()
                                    .WaitForDeclinedPage();
       }

    }
}
