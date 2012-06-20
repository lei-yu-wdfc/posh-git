using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.Enums.Risk.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class L0LoanUk : UiTest
    {

        // Check L0 loan is accepted and Loan Agreement is displayed
        // Check L0 loan is completed and text on Deal Done page is correct
        [Test, AUT(AUT.Uk), JIRA("UK-730", "UK-731"), MultipleAsserts]
        public void L0AcceptedCompleted()
        {
            string expectedDealDoneText = "Your application has been accepted\r\nThe cash will be winging its way into your bank account in the next 15 minutes! Please just be aware that different banks take different lengths of time to show new deposits.\r\nPlease don\'t forget that you have promised to repay on {repay date} when you\'ll need to have £{repay amount} ready in the bank account linked to your debit card. You can login to your Wonga account at any time to keep track of your loan, apply for more cash (depending on your trust rating) and even extend or repay early.\r\nWe hope you find the money useful and, if you love our service, please now check out the options below!";
            const int loanAmount = 100;
            const int days = 10;
            string paymentAmount = 115.91M.ToString("#.00");
            DateTime paymentDate = DateTime.Now.AddDays(days);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAmount(loanAmount).WithDuration(days);

            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            Assert.IsTrue(acceptedPage.IsAgreementFormDisplayed());

            var dealDonePage = journey.Teleport<DealDonePage>() as DealDonePage;
            string actualDealDoneText = dealDonePage.GetDealDonePageText;
            expectedDealDoneText = expectedDealDoneText.Replace("{repay date}", Date.GetOrdinalDate(paymentDate, "dddd d MMM yyyy")).Replace("{repay amount}", paymentAmount);
            Assert.AreEqual(expectedDealDoneText, actualDealDoneText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-438", "UK-1823", "UKWEB-253"), Pending("Stops on Accept page. Then also opens Success page, not Decline")]
        public void L0DeclinedForEmployedPartTimeTest()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAmount(400).WithDuration(30)
                .WithDeclineDecision();
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-438", "UK-1823")]
        [Pending("Enable if we need to simulate different Employment statuses for declined loan")]
        public void L0DeclinedForNotFullEmployedTest([EnumData(typeof(EmploymentStatusEnum), ExcludeArray = new object[] { EmploymentStatusEnum.EmployedFullTime })] EmploymentStatusEnum employmentStatus)
        {
            string email = Get.RandomEmail();

            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email)
                .WithAmount(400).WithDuration(30);
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }
    }
}
