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

        BasePage Teleport<T>();

        IL0ConsumerJourney ApplyForLoan(bool submit = true);
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

        IL0ConsumerJourney AnswerEligibilityQuestions(bool submit = true);
        IL0ConsumerJourney EnterBusinessDetails(bool submit = true);
        IL0ConsumerJourney DeclineAddAdditionalDirector(bool submit = true);
        IL0ConsumerJourney AddAdditionalDirector(bool submit = true);
        IL0ConsumerJourney EnterBusinessBankAccountDetails(bool submit = true);
        IL0ConsumerJourney EnterBusinessDebitCardDetails(bool submit = true);
        IL0ConsumerJourney WaitForApplyTermsPage(bool submit = true);
        IL0ConsumerJourney ApplyTerms(bool submit = true);
        IL0ConsumerJourney GoHomePage(bool submit = true);

        #region Builder

        IL0ConsumerJourney FillAndStop();
        IL0ConsumerJourney WithDeclineDecision();

        IL0ConsumerJourney WithAmount(int amount);
        IL0ConsumerJourney WithDuration(int duration);

        IL0ConsumerJourney WithFirstName(string firstName);
        IL0ConsumerJourney WithLastName(string lastName);
        IL0ConsumerJourney WithMiddleName(string middleName);
        IL0ConsumerJourney WithTitle(string title);
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


        IL0ConsumerJourney WithAutAdditionalDirrector();
        IL0ConsumerJourney WithEligibilityQuestions(bool activeCompany = true, bool director = true, bool guarantee = true, bool resident = true, bool debitCard = true);
        IL0ConsumerJourney WithAdditionalDirectorName(string additionalDirectorName);
        IL0ConsumerJourney WithAdditionalDirectorSurName(string additionalDirectorSurName);
        IL0ConsumerJourney WithAdditionalDirectorEmail(string additionalDirectorEmail);
        IL0ConsumerJourney WithBusinessBankAccount(string businessBankAccount);
        IL0ConsumerJourney WithBusinessBankPeriod(string businessBankPeriod);
        IL0ConsumerJourney WithBusinessDebitCardNumber(string businessDebitCardNumber);
        IL0ConsumerJourney WithBusinessDebitCardSecurity(string businessDebitCardSecurity);
        IL0ConsumerJourney WithBusinessDebitCardType(string businessDebitCardType);
        IL0ConsumerJourney WithBusinessDebitCardExpiryDate(string businessDebitExpiryDate);
        IL0ConsumerJourney WithBusinessDebitCardStartDate(string businessDebitStartDate);

        #endregion
    }
}
