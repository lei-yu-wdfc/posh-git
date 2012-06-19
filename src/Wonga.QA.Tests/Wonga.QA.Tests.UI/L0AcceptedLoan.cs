using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class L0AcceptedLoan : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

        [Test, AUT(AUT.Za, AUT.Ca), SmokeTest]
        public void ZaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home()).WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var processingPage = journey.Teleport<MySummary>() as MySummaryPage;
        }

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoan()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithMiddleName(MiddleNameMask)
               .WithAddresPeriod("More than 4 years");
           var homePage = journey.Teleport<HomePage>() as HomePage;
       }

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoanAddAdditionalDirector()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName(MiddleNameMask)
               .WithAddresPeriod("2 to 3 years")
               .WithAdditionalDirrector();
           var homePage = journey.Teleport<HomePage>() as HomePage;
       }

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoanUpdateLoanDurationOnApplyTermsPage()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithMiddleName(MiddleNameMask)
               .WithAddresPeriod("3 to 4 years");
           var homePage = journey.Teleport<HomePage>() as HomePage;
       }     

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoanAddressLessThan2Years()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithMiddleName(MiddleNameMask)
               .WithAddresPeriod("Between 4 months and 2 years");
           var homePage = journey.Teleport<HomePage>() as HomePage;

       }

        [Test, AUT(AUT.Uk)]
        public void UkAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home()).WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

        }

        [Test, AUT(AUT.Uk), JIRA("UK-730")]
        public void CheckLoanAgreement()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home()).WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));

            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;

            Assert.IsTrue(acceptedPage.IsAgreementFormDisplayed());

        }

        [Test, AUT(AUT.Uk), JIRA("UK-731")]
        public void LoanCompletionConfirmed()
        {
            string expectedDealDoneText = "Your application has been accepted\r\nThe cash will be winging its way into your bank account in the next 15 minutes! Please just be aware that different banks take different lengths of time to show new deposits.\r\nPlease don\'t forget that you have promised to repay on {repay date} when you\'ll need to have £{repay amount} ready in the bank account linked to your debit card. You can login to your Wonga account at any time to keep track of your loan, apply for more cash (depending on your trust rating) and even extend or repay early.\r\nWe hope you find the money useful and, if you love our service, please now check out the options below!";
            const int loanAmount = 100;
            const int days = 10;
            string paymentAmount = 115.91M.ToString("#.00");
            DateTime paymentDate = DateTime.Now.AddDays(days);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAmount(loanAmount).WithDuration(days);

            var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;

            string actualDealDoneText = dealDonePage.GetDealDonePageText;

            // Check text on the Deal Done page is displayed correctly
            expectedDealDoneText = expectedDealDoneText.Replace("{repay date}", Date.GetOrdinalDate(paymentDate, "dddd d MMM yyyy")).Replace("{repay amount}", paymentAmount);
            Assert.AreEqual(expectedDealDoneText, actualDealDoneText);
        }
    }
}
