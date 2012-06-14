using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{

    public interface IL0ConsumerJourney
    {
      // String FirstName { get; set; }
      // String LastName { get; set; }
        String NationalId { get; set; }
        DateTime DateOfBirth { get; set; }

        BasePage CurrentPage { get; set; }

        IL0ConsumerJourney ApplyForLoan(int amount, int duration);
        IL0ConsumerJourney FillPersonalDetails(bool submit = true);
        IL0ConsumerJourney FillAddressDetails(string postcode = null, string addresPeriod = null, bool submit = true);
        IL0ConsumerJourney FillAccountDetails(string password = null, bool submit = true);
        IL0ConsumerJourney FillBankDetails(string accountNumber = null, string bankPeriod = null, string pin = null, bool submit = true);
        IL0ConsumerJourney FillCardDetails(string cardNumber = null, string cardSecurity = null, string cardType = null, string expiryDate = null, string startDate = null, string pin = null, bool submit = true);
        IL0ConsumerJourney WaitForAcceptedPage();
        IL0ConsumerJourney WaitForDeclinedPage();
        IL0ConsumerJourney FillAcceptedPage();
        IL0ConsumerJourney GoToMySummaryPage();
        IL0ConsumerJourney IgnoreAcceptingLoanAndReturnToHomePageAndLogin();

        #region Builder

        IL0ConsumerJourney WithSubmit(bool submit);

        IL0ConsumerJourney WithFirstName(string firstName);
        IL0ConsumerJourney WithLastName(string lastName);
        IL0ConsumerJourney WithMiddleName(string middleName);
        IL0ConsumerJourney WithEmployerName(string employerName);
        IL0ConsumerJourney WithEmail(string email);
        IL0ConsumerJourney WithMobilePhone(string mobilePhone);
        IL0ConsumerJourney WithGender(GenderEnum gender);
        IL0ConsumerJourney WithDateOfBirth(DateTime dateOfBirth);
        IL0ConsumerJourney WithNationalId(string nationalId);

        #endregion
    }
}
