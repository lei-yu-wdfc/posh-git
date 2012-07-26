using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Ca.Journey
{
    [Parallelizable(TestScope.All)]
    public class L0LoanCa : UiTest
    {
        [Test, AUT(AUT.Ca), Category(TestCategories.SmokeTest)]
        public void CaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)); 
            var mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

       [Test, AUT(AUT.Ca)]
       public void CaDeclinedLoan()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithDeclineDecision();
           var processingPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
       }

    }
}
