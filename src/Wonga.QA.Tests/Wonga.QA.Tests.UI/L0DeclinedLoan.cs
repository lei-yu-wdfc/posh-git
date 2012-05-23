using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    public class L0DeclinedLoan : UiTest
    {
        [Test, AUT(AUT.Wb)]
        public void WbDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
            journey.ApplyForLoan(5500, 30)
                .AnswerEligibilityQuestions()
                .FillPersonalDetails()
                .FillAddressDetails("More than 4 years")
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .EnterBusinessDetails()
                .DeclineAddAdditionalDirector()
                .EnterBusinessBankAccountDetails()
                .EnterBusinessDebitCardDetails()
                .WaitForDeclinedPage();
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

        [Test, AUT(AUT.Ca)]
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

 
        [Test, AUT(AUT.Uk), JIRA("UK-438", "UK-1823")]
        public void L0DeclinedForEmployedPartTimeTest()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            var processingPage = journeyL0.ApplyForLoan(400, 30)
                .FillPersonalDetails(Get.EnumToString(EmploymentStatusEnum.EmployedPartTime))
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .CurrentPage as ProcessingPage;

            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-438", "UK-1823")]
        [Pending("Enable if we need to simulate different Employment statuses for declined loan")]
        public void L0DeclinedForNotFullEmployedTest([EnumData(typeof(EmploymentStatusEnum), ExcludeArray = new object[] { EmploymentStatusEnum.EmployedFullTime })] EmploymentStatusEnum employmentStatus)
        {
            string email = Get.RandomEmail();

            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            var processingPage = journeyL0.ApplyForLoan(400, 30)
                .FillPersonalDetailsWithEmail(Get.EnumToString(employmentStatus), email)
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .CurrentPage as ProcessingPage;

            var declinedPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());
        }


        /*  TBD 1: how can we test different types of advices:
            GeneralResolutionAdvice,
            BankAccountMatchAdvice,
            CreditBureauMissDetailsAdvice,
            CheckCreditHistoryAdvice,
            CheckDateOfBirthAdvice,
            CheckFinancialHealthAdvice,
            CheckPaymentCardDetailsAdvice,
            RepaymentAbilityAdvice
         * 
         * TBD 2: what other conditions, apart from Employment status, can we simulate?
         * TBD 3: is this correct that for different Employment statuses we get the same, very general, advice?
         * TBD 4: can we simulate combination of conditions leading to a decined loan?
         * TBD 5: is class DeclinedPage only for L0 journey? If it will be different for other journeys, can we rename it to L0DeclinedPage? Is there DeclinedPage for Ln journey?
         * TBD 6: it seems no matter which Employment type we simulate, we still can open the Declined page. Is that correct? How can we ensure that the Declined page opens only for appropriate conditions?
         * TBD 7: for which Emplyment statuses do we expect a declined loan?
         */

    }
}
