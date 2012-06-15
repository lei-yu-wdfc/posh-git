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
        BasePage CurrentPage { get; set; }

        // IL0ConsumerJourney Teleport<T>();

        IL0ConsumerJourney ApplyForLoan(bool submit);
        IL0ConsumerJourney FillPersonalDetails(bool submit = true);
        IL0ConsumerJourney FillAddressDetails(bool submit = true);
        IL0ConsumerJourney FillAccountDetails(bool submit = true);
        IL0ConsumerJourney FillBankDetails(bool submit = true);
        IL0ConsumerJourney FillCardDetails(bool submit = true);
        IL0ConsumerJourney WaitForAcceptedPage(bool submit = true);
        IL0ConsumerJourney WaitForDeclinedPage(bool submit = true);
        IL0ConsumerJourney FillAcceptedPage(bool submit = true);
        IL0ConsumerJourney GoToMySummaryPage(bool submit = true);
        IL0ConsumerJourney IgnoreAcceptingLoanAndReturnToHomePageAndLogin(bool submit = true);


        #region Builder

        IL0ConsumerJourney FillAndStop();
        IL0ConsumerJourney WithDeclineDecision();

        IL0ConsumerJourney WithAmount(int amount);
        IL0ConsumerJourney WithDuration(int duration);

        IL0ConsumerJourney WithFirstName(string firstName);
        IL0ConsumerJourney WithLastName(string lastName);
        IL0ConsumerJourney WithMiddleName(string middleName);
        IL0ConsumerJourney WithEmployerName(string employerName);
        IL0ConsumerJourney WithEmail(string email);
        IL0ConsumerJourney WithMobilePhone(string mobilePhone);
        IL0ConsumerJourney WithGender(GenderEnum gender);
        IL0ConsumerJourney WithDateOfBirth(DateTime dateOfBirth);
        IL0ConsumerJourney WithNationalId(string nationalId);
        IL0ConsumerJourney WithMotherMaidenName(string motherMaidenName);

        IL0ConsumerJourney WithPosteCode(string postCode);
        IL0ConsumerJourney WithAddresPeriod(string addresPeriod);

        IL0ConsumerJourney WithPassword(string password);

        IL0ConsumerJourney WithAccountNumber(string accountNumber);
        IL0ConsumerJourney WithBankPeriod(string bankPeriod);
        IL0ConsumerJourney WithPin(string pin);

        IL0ConsumerJourney WithCardNumber(string cardNumber);
        IL0ConsumerJourney WithCardSecurity(string cardSecurity);
        IL0ConsumerJourney WithCardType(string cardType);
        IL0ConsumerJourney WithExpiryDate(string expiryDate);
        IL0ConsumerJourney WithStartDate(string startDate);

        #endregion
    }
}
