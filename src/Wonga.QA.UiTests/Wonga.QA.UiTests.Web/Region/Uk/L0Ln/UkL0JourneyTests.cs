using System;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Uk.L0Ln
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    public class UkL0JourneyTests : UiTest
    {
        Dictionary<RiskMaskForDeclinedLoan, string> DeclineAdvices = new Dictionary<RiskMaskForDeclinedLoan, string> 
     {
         {RiskMaskForDeclinedLoan.TESTBankAccountMatchedToApplicant, "Your bank details don't match your personal information\r\n\r\nWe've been unable to match the bank details you submitted with the rest of your personal information. If you suspect this may just be an error made when filling out the form, please check and update your details here before applying again. Or you could try speaking to your bank if you suspect their records might need updating. We also recommend ordering a copy of your credit report to check the status and accuracy of your personal credit history. Please find the contact details below to obtain a free copy of your credit report."}
        };

        public enum RiskMaskForDeclinedLoan
        {
            TESTBankAccountMatchedToApplicant,
            /*TESTDateOfBirth,
            TESTCustomerDateOfBirthIsCorrect,
            TESTBankAccountHistoryIsAcceptable,
            TESTPaymentCardHistoryIsAcceptable,
            TESTTransUnionandBank,
            TESTTransUnion,
            TESTEmployedMask,
            TESTCardMask,
            TESTCardBankMask,
            TESTAll,
            TESTExcludeVerification,
            TESTBlacklist,
            TESTIsAlive,
            TESTIsSolvent,
            TESTMonthlyIncome,
            TESTAccountNumberApplicationsAcceptable,
            TESTCustomerHistoryIsAcceptable,
            TESTApplicationElementNotOnBlacklist,
            TESTDirectFraud,
            TESTApplicationElementNotCIFASFlagged,
            TESTCreditBureauDataIsAvailable,
            TESTApplicantIsNotDeceased,
            TESTCustomerIsEmployed,
            TESTCustomerIsSolvent,
            TESTCustomerDateOfBirthIsCorrectSME,
            TESTFraudScorePositive,
            TESTDirectFraudCheck,
            TESTCreditBureauScoreIsAcceptable,
            TESTApplicationElementNotOnCSBlacklist,
            TESTApplicationDeviceNotOnBlacklist,
            TESTDeviceNotOnBlacklist,
            TESTMonthlyIncomeEnoughForRepayment,
            TESTPaymentCardIsValid,
            TESTRepaymentPredictionPositive,
            TESTReputationtPredictionPositive,
            TESTNoSuspiciousApplicationActivity,
            TESTCallValidateBankAccountMatchedToApplicant,
            TESTCallValidatePaymentCardIsValid,
            TESTExperianBankAccountMatchedToApplicant,
            TESTExperianPaymentCardIsValid,
            TESTRiskBankAccountMatchedToApplicant,
            TESTRiskPaymentCardIsValid,
            TESTRiskFraudScorePositive,
            TESTExperianCreditBureauDataIsAvailable,
            TESTExperianApplicationElementNotCIFASFlagged,
            TESTExperianApplicantIsNotDeceased,
            TESTExperianCustomerIsSolvent,
            TESTExperianCustomerDateOfBirthIsCorrect,
            TESTExperianCustomerDateOfBirthIsCorrectSME,
            TESTManualReferralIovation,
            TESTManualReferralCIFAS,
            TESTManualReferralFraudScore,
            TESTCustomerNameIsCorrect,
            TESTMobilePhoneIsUnique,
            TESTApplicantIsNotMinor,
            TESTBankAccountIsValid,
            TESTEquifaxCreditBureauDataIsAvailable,
            TESTHomePhoneIsAcceptable,
            TESTBusinessPaymentScoreIsAcceptable,
            TESTBusinessIsCurrentlyTrading,
            TESTBusinessBureauDataIsAvailable,
            TESTMainApplicantMatchesBusinessBureauData,
            TESTBusinessPerformanceScoreIsAcceptaple,
            TESTMainApplicantDurationAcceptable,
            TESTNumberOfDirectorsMatchesBusinessBureauData,
            TESTBusinessDateOfIncorporationAcceptable,
            TESTNoCheck,
            TESTTooManyLoansAtAddress,
            TESTGuarantorNamesMatchBusinessBureauData,
            TESTBlacklistSME,
            TESTGeneralManualVerification,
            TESTDoNotRelend,
            TESTFraudBlacklist,
            TESTApplicantHasPoorRelationshipWithWonga,
            TESTApplicantIsNotMinorUru,
            TESTCardLivePaymentCardVerification,*/
        }

        // Check L0 loan is accepted and Loan Agreement is displayed
        // Check L0 loan is completed and text on Deal Done page is correct
        [Test, JIRA("UK-730", "UK-731"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
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

        [Test, JIRA("UKWEB-253"), Owner(Owner.StanDesyatnikov)]
        public void L0Declined()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithAmount(400).WithDuration(30)
                .WithDeclineDecision();
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }

        [Test, JIRA("UKWEB-253"), Owner(Owner.StanDesyatnikov, Owner.PavithranVangiti)]
        public void L0DeclinedWithVariousAdvices([EnumData(typeof(RiskMaskForDeclinedLoan))] RiskMaskForDeclinedLoan riskMask)
        {
            var email = Get.RandomEmail();
            Console.WriteLine("Email: {0}", email);
            Console.WriteLine("riskMask: {0}", riskMask);

            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmail(email)
                .WithEmployerName(Get.EnumToString(riskMask))
                .WithAmount(400).WithDuration(30)
                .WithDeclineDecision();
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
            Assert.AreEqual(DeclineAdvices[riskMask], declinedPage.DeclineAdvice());
            Console.WriteLine("L0 Decline Advice: {0}", declinedPage.DeclineAdvice());

            // TODO: check that "here" link in Decline Advice leads to correct page. Now it leads to Wonga.com/my-account, which does not exists.

            /* TODO: Ln fails with Nearly There page instead of Decline Page
            // log in
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            var journeyLn = JourneyFactory.GetLnJourney(Client.Home());
            declinedPage = journeyLn.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
            Console.WriteLine("Ln Decline Advice: {0}", declinedPage.DeclineAdvice());*/
        }

        /* TODO: check if it is possible to simulate different employment statuses
         * [Test, JIRA("UK-438", "UK-1823"), Owner(Owner.StanDesyatnikov)]
        [Pending("Enable if we need to simulate different Employment statuses for declined loan")]
        public void L0DeclinedForNotFullEmployedTest([EnumData(typeof(EmploymentStatusEnum), ExcludeArray = new object[] { EmploymentStatusEnum.EmployedFullTime })] EmploymentStatusEnum employmentStatus)
        {
            string email = Get.RandomEmail();

            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email)
                .WithAmount(400).WithDuration(30);
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }*/

        [Test, JIRA("UK-969", "UKWEB-250"), MultipleAsserts, Owner(Owner.StanDesyatnikov)]
        [Pending ("UKWEB-1143: Document links are not working in Account Setup Page")]
        public void L0AccountSetupPageDocumentLinksShouldOpenPopups()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            int loanAmount = 200;
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithAmount(loanAmount)
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithEmail(email);
            var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
            Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
            Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

            Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
            accountSetupPage.ClosePopupWindow();

            Thread.Sleep(1000);

            Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
            accountSetupPage.ClosePopupWindow();

            Thread.Sleep(1000);

            accountSetupPage.ClickSecciLink();
            Assert.Contains(accountSetupPage.SecciPopupWindowContent(), loanAmount.ToString("#"));
            accountSetupPage.ClosePopupWindow();
        }
    }
}
