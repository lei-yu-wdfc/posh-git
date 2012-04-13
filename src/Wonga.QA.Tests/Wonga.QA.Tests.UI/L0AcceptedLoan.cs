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
    public class L0AcceptedLoan : UiTest
    {
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
            var processingPage = WbL0Path("TESTNoCheck");
            var applyTermsPage = processingPage.WaitFor<ApplyTermsPage>() as ApplyTermsPage;
            applyTermsPage.EditDurationOfLoan("22");
            
            var acceptedPage = applyTermsPage.Next();
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
           
            var referPage = acceptedPage.Submit() as ReferPage;
            var homepage = referPage.GoHome();
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

        [Test, AUT(AUT.Uk), JIRA("UK-731")]
        public void LoanCompletionConfirmed()
        {
            const int loanAmount = 100;
            const int days = 10;
            string paymentAmount = 115.91M.ToString("#.00");
            DateTime paymentDate = DateTime.Now.AddDays(days);

            var journey = JourneyFactory.GetL0Journey(Client.Home());

            var dealDonePage = journey.ApplyForLoan(loanAmount, days)
                                   .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                   .FillAddressDetails()
                                   .FillAccountDetails()
                                   .FillBankDetails()
                                   .FillCardDetails()
                                   .WaitForAcceptedPage()
                                   .FillAcceptedPage().CurrentPage as DealDonePage;

            string actualDealDoneText = dealDonePage.GetDealDonePageText;

            // Check text on the Deal Done page is displayed correctly
            Assert.Contains(actualDealDoneText, "The cash will be winging its way into your bank account in the next 15 minutes!");
            Assert.Contains(actualDealDoneText, paymentAmount);
            Assert.Contains(actualDealDoneText, paymentDate.ToString("dd"));
            Assert.Contains(actualDealDoneText, paymentDate.ToString("MMM"));
            Assert.Contains(actualDealDoneText, paymentDate.ToString("yyyy"));
            Assert.Contains(actualDealDoneText, paymentDate.ToString("dddd"));
        }
    }
}
