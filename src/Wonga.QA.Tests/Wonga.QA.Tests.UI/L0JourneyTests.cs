using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    class L0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-180")]
        public void L0JourneyInvalidPostcodeShouldCauseWarningMessage()
        {
            var journey = new Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails("test:EmployedMask")
                                      .CurrentPage as AddressDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    addressPage.PostCode = "qqqq";
                    break;
                case AUT.Ca:
                    addressPage.PostCode = "111111";
                    break;
            }
            addressPage.Street = "Asd"; //to lost focus
            Assert.IsTrue(addressPage.IsPostcodeWarningOccurred());

        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-189")]
        public void L0JourneyInvalidPINShouldCauseWarningMessageOnNextPage()
        {
            var journey = new Journey(Client.Home());
            var bankDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails("test:EmployedMask")
                                      .FillAddressDetails()
                                      .FillAccountDetails()
                                      .CurrentPage as PersonalBankAccountPage;

            switch (Config.AUT)
            {
                case AUT.Za:
                    bankDetailsPage.BankAccountSection.BankName = "Capitec";
                    bankDetailsPage.BankAccountSection.BankAccountType = "Current";
                    bankDetailsPage.BankAccountSection.AccountNumber = "1234567";
                    bankDetailsPage.BankAccountSection.BankPeriod = "2 to 3 years";
                    break;
                case AUT.Ca:
                    bankDetailsPage.BankAccountSection.BankName = "Bank of Montreal";
                    bankDetailsPage.BankAccountSection.BranchNumber = "00011";
                    bankDetailsPage.BankAccountSection.AccountNumber = "3023423";
                    bankDetailsPage.BankAccountSection.BankPeriod = "More than 4 years";
                   break;
            }
            bankDetailsPage.PinVerificationSection.Pin = "9999";
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage.Next(); });
            
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-177"), Pending("Not completed yet")]
        public void ChangeLoanAmountAndDurationOnPersonalDetailsViaPlusMinusOptions()
        {
            var journey = new Journey(Client.Home());
            var personalDetailsPage = journey.ApplyForLoan(200, 10).CurrentPage as PersonalDetailsPage;
            personalDetailsPage.ClickSliderToggler();
            personalDetailsPage.Sliders.ClickAmountPlusButton();
            personalDetailsPage.Sliders.ClickDurationMinusButton();
            
            Assert.IsNotNull(personalDetailsPage);
        }
    }
}
