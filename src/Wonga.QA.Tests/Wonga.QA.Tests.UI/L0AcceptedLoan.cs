using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Ui
{
    public class L0AcceptedLoan : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home()); 
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
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
            var journey = JourneyFactory.GetL0Journey(Client.Home()); 
            var processingPage = journey.ApplyForLoan(200, 10)
                                 .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                 .FillAddressDetails()
                                 .FillAccountDetails()
                                 .FillBankDetails()
                                 .CurrentPage as ProcessingPage;

            var acceptedPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            acceptedPage.SignConfirmCaL0(DateTime.Now.ToString("d MMM yyyy"), _firstName, _lastName);
            var dealDone = acceptedPage.Submit();
        }

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoan()
       {
           var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
           var homePage = journey.ApplyForLoan(5500, 30)
               .AnswerEligibilityQuestions()
               .FillPersonalDetails(MiddleNameMask)
               .FillAddressDetails("More than 4 years")
               .FillAccountDetails()
               .FillBankDetails()
               .FillCardDetails()
               .EnterBusinessDetails()
               .DeclineAddAdditionalDirector()
               .EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForApplyTermsPage()
               .ApplyTerms()
               .FillAcceptedPage()
               .GoHomePage();
       }

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoanAddAdditionalDirector()
       {
           var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
           var homePage = journey.ApplyForLoan(5500, 30)
               .AnswerEligibilityQuestions()
               .FillPersonalDetails(MiddleNameMask)
               .FillAddressDetails("2 to 3 years")
               .FillAccountDetails()
               .FillBankDetails()
               .FillCardDetails()
               .EnterBusinessDetails()
               .AddAdditionalDirector()
               .EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForApplyTermsPage()
               .ApplyTerms()
               .FillAcceptedPage()
               .GoHomePage();
       }

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoanUpdateLoanDurationOnApplyTermsPage()
       {
           var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
           var homePage = journey.ApplyForLoan(5500, 30)
               .AnswerEligibilityQuestions()
               .FillPersonalDetails(MiddleNameMask)
               .FillAddressDetails("3 to 4 years")
               .FillAccountDetails()
               .FillBankDetails()
               .FillCardDetails()
               .EnterBusinessDetails()
               .DeclineAddAdditionalDirector()
               .EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForApplyTermsPage()
               .UpdateLoanDuration()
               .ApplyTerms()
               .FillAcceptedPage()
               .GoHomePage();
       }     

       [Test, AUT(AUT.Wb)]
       public void WbAcceptedLoanAddressLessThan2Years()
       {
           var journey = JourneyFactory.GetL0JourneyWB(Client.Home());
           var homePage = journey.ApplyForLoan(5500, 30)
               .AnswerEligibilityQuestions()
               .FillPersonalDetails(MiddleNameMask)
               .FillAddressDetails("Between 4 months and 2 years")
               .EnterAdditionalAddressDetails()
               .FillAccountDetails()
               .FillBankDetails()
               .FillCardDetails()
               .EnterBusinessDetails()
               .DeclineAddAdditionalDirector()
               .EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForApplyTermsPage()
               .ApplyTerms()
               .FillAcceptedPage()
               .GoHomePage();

       }

        [Test, AUT(AUT.Uk)]
        public void UkAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            var acceptedPage = journey.ApplyForLoan(200, 10)
                                     .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                     .FillAddressDetails()
                                     .FillAccountDetails()
                                     .FillBankDetails()
                                     .FillCardDetails()
                                     .WaitForAcceptedPage().CurrentPage as AcceptedPage;

        }

        [Test, AUT(AUT.Uk), JIRA("UK-730")]
        public void CheckLoanAgreement()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            var acceptedPage = journey.ApplyForLoan(200, 10)
                                     .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                     .FillAddressDetails()
                                     .FillAccountDetails()
                                     .FillBankDetails()
                                     .FillCardDetails()
                                     .WaitForAcceptedPage().CurrentPage as AcceptedPage;

            Assert.IsTrue(acceptedPage.IsAgreementFormDisplayed());

        }


    }
}
