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
    public class L0LoanWb : UiTest
    {
        private const String MiddleNameMask = "TESTNoCheck";

        [Test, AUT(AUT.Wb)]
        public void WbAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithMiddleName(MiddleNameMask)
                .WithAddresPeriod("More than 4 years");
            var homePage = journey.ApplyForLoan()
               .AnswerEligibilityQuestions()
               .FillPersonalDetails()
               .FillAddressDetails()
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
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithMiddleName(MiddleNameMask)
                .WithAddresPeriod("2 to 3 years");
           var homePage = journey.ApplyForLoan()
               .AnswerEligibilityQuestions()
               .FillPersonalDetails()
               .FillAddressDetails()
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
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithMiddleName(MiddleNameMask)
               .WithAddresPeriod("3 to 4 years");
           var homePage = journey.ApplyForLoan()
               .AnswerEligibilityQuestions()
               .FillPersonalDetails()
               .FillAddressDetails()
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
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithMiddleName(MiddleNameMask)
               .WithAddresPeriod("Between 4 months and 2 years");
           var homePage = journey.ApplyForLoan()
               .AnswerEligibilityQuestions()
               .FillPersonalDetails()
               .FillAddressDetails()
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
       public void WbDeclinedLoan()
       {
           var journey = JourneyFactory.GetL0Journey(Client.Home())
               .WithAddresPeriod("More than 4 years");
           journey.ApplyForLoan()
               .AnswerEligibilityQuestions()
               .FillPersonalDetails()
               .FillAddressDetails()
               .FillAccountDetails()
               .FillBankDetails()
               .FillCardDetails()
               .EnterBusinessDetails()
               .DeclineAddAdditionalDirector()
               .EnterBusinessBankAccountDetails()
               .EnterBusinessDebitCardDetails()
               .WaitForDeclinedPage();
       }

    }
}
