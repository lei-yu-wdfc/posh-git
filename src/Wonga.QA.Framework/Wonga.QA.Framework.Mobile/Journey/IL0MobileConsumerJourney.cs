using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework.Mobile.Ui.Pages;

//using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.Mobile.Journey
{

    public interface IL0MobileConsumerJourney
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        String Gender { get; set; } /// needed for migation testing
        String NationalId { get; set; }
        DateTime DateOfBirth { get; set; }

        BasePageMobile CurrentPage { get; set; }

        IL0MobileConsumerJourney ApplyForLoan(int amount, int duration);
        IL0MobileConsumerJourney FillPersonalDetails(string firstName = null, string lastName = null, string middleNameMask = null, string gender = null, string employerNameMask = null, string email = null, string mobilePhone = null, bool submit = true);
        IL0MobileConsumerJourney FillAddressDetails(string postcode = null, string addresPeriod = null, bool submit = true);
        IL0MobileConsumerJourney FillAccountDetails(string password = null, bool submit = true);
        IL0MobileConsumerJourney FillBankDetails(string accountNumber = null, string bankPeriod = null, string pin = null, bool submit = true);
        IL0MobileConsumerJourney FillCardDetails(string cardNumber = null, string cardSecurity = null, string cardType = null, string expiryDate = null, string startDate = null, string pin = null, bool submit = true);
        IL0MobileConsumerJourney WaitForAcceptedPage();
        IL0MobileConsumerJourney WaitForDeclinedPage();
        IL0MobileConsumerJourney FillAcceptedPage();
        IL0MobileConsumerJourney GoToMySummaryPage();
        IL0MobileConsumerJourney IgnoreAcceptingLoanAndReturnToHomePageAndLogin();
    }
}
