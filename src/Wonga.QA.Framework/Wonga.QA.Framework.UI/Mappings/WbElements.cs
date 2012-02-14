using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Mappings.Pages;
using Wonga.QA.Framework.UI.Mappings.Pages.Wb;
using Wonga.QA.Framework.UI.Mappings.Sections;

namespace Wonga.QA.Framework.UI.Mappings
{
    internal sealed class WbElements : BaseElements
    {
        internal WbElements()
        {
            #region ElementsMappings

            YourNameElement = new YourNameElement
                                  {
                                      Legend = "Personal Details|Your name",
                                      Title = "title",
                                      FirstName = "first_name",
                                      MiddleName = "middle_name",
                                      LastName = "last_name"
                                  };

            YourDetailsElement = new YourDetailsElement
                                     {
                                         Legend = "Personal Details|Your details",
                                         IdNumber = "id_number",
                                         Dependants = "dependants",
                                         Gender = "gender",
                                         DateOfBirthDay = "date_of_birth[day]",
                                         DateOfBirthMonth = "date_of_birth[month]",
                                         DateOfBirthYear = "date_of_birth[year]",
                                         HomeStatus = "home_status",
                                         MaritalStatus = "marital_status"
                                     };

            MobilePinVerificationElement = new MobilePinVerificationElement { Pin = "pin", Legend = "Mobile PIN verification" };

            ContactingYouElement = new ContactingYouElement
                                       {
                                           Legend = "Contacting you",
                                           Email = "email",
                                           EmailConfirm = "email_confirm",
                                           HomePhone = "home_phone",
                                           MobilePhone = "mobile_phone"
                                       };

            DebitCardElement = new DebitCardElement
                                          {
                                              Legend = "Card details|card details",
                                              CardType = "card_type",
                                              CardNumber = "card_number",
                                              CardName = "card_name",
                                              CardExpiryDateMonth = "card_expiry_date[month]",
                                              CardExpiryDateYear = "card_expiry_date[year]",
                                              CardSecurityNumber = "card_security",
                                              CardStartDateMonth = "card_start_date[month]",
                                              CardStartDateYear = "card_start_date[year]"
                                          };

            BankAccountElement = new BankAccountElement
                                            {
                                                Legend = "your bank details|business bank account",
                                                BankName = "bank_name",
                                                SortCodePart1 = "sort_code[part1]",
                                                SortCodePart2 = "sort_code[part2]",
                                                SortCodePart3 = "sort_code[part3]",
                                                AccountNumber = "account_number",
                                                BankPeriod = "bank_period"
                                            };

            SliderElement = new SliderElement
                                {
                                    FormId = "wonga-sliders-form",
                                    LoanAmount = "loan_amount",
                                    LoanDuration = "loan_duration",
                                    SubmitButton = "op"
                                };

            AccountDetailsElement = new AccountDetailsElement
                                        {
                                            Legend = "account setup",
                                            Password = "password",
                                            PasswordConfirm = "password_confirm",
                                            SecretQuestion = "secret_question",
                                            SecretAnswer = "secret_answer"
                                        };

            #endregion

            #region WbPagesMappings

            WbEligibilityQuestionsPage = new EligibilityQuestionsPage
                                             {
                                                 FormId = "lzero-questions-form",
                                                 CheckDirector = "director",
                                                 CheckResident = "resident",
                                                 CheckActiveCompany = "active_company",
                                                 CheckTurnover = "turnover",
                                                 CheckVat = "vat",
                                                 CheckOnlineAccess = "online_access",
                                                 CheckGuarantee = "guarantee",
                                                 NextButton = "next"
                                             };

            PersonalDetailsPage = new PersonalDetailsPage
                                      {
                                          FormId = "lzero-personal-form",
                                          CheckPrivacyPolicy = "privacy",
                                          CheckCanContact = "update_option",
                                          NextButton = "next"
                                      };

            WbAddressDetailsPage = new AddressDetailsPage
                                       {
                                           FormId = "lzero-address-form",
                                           LookupButton = "op",
                                           PostCode = "postcode_lookup_uk",
                                           District = "district",
                                           FlatNumber = "flat",
                                           County = "county",
                                           AddressPeriod = "address_period",
                                           NextButton = "next",
                                           AddressOptions = "address_options"
                                       };

            WbBusinessAccountDetailsPage = new BusinessAccountPage { FormId = "lzero-account-setup-form", NextButton = "next" };

            WbPersonalBankAccountPage = new PersonalBankAccountDetailsPage {FormId = "lzero-bank-form", NextButton = "next"};

            WbPersonalDebitCardDetailsPage = new PersonalDebitCardPage {FormId = "lzero-card-form", NextButton = "next"};

            WbBusinessDetailsPage = new BusinessDetailsPage
                                        {
                                            FormId = "lzero-business-form",
                                            NextButton = "next",
                                            BusinessNumber = "biz_number",
                                            BusinessName = "biz_name"
                                        };

            WbAdditionalDirectorsPage = new AdditionalDirectorsPage
                                            {
                                                FormId = "lzero-directors-form",
                                                DoneButton = "done",
                                                AddAnotherDirector = "add"
                                            };

            WbBusinessBankAccountPage = new BusinessBankAccountPage {FormId = "lzero-business-bank-form", NextButton = "next"};

            WbBusinessDebitCardPage = new BusinessDebitCardPage {FormId = "lzero-business-card-form", NextButton = "next"};

            ProcessingPage = new ProcessingPage
                                 {
                                     FormId = "wonga-processing",
                                     Legend = "processing your application",
                                     ProcessingImageTag = "img",
                                     ProcessingImageAttributeName = "alt",
                                     ProcessingImageAttributeText = "Processing"
                                 };

            WbAcceptedPage = new AcceptedPage
                                 {
                                     FormId = "wonga-loan-approve-form",
                                     AcceptBusinessLoan = "terms-accept",
                                     AcceptGuarantorLoan = "guarantor-accept",
                                     SubmitButton = "op",
                                     AcceptLinkText = "I Accept"
                                 };

            DealDonePage = new DealDonePage {HeaderText = "Application success", ContinueButtonLinkText = "Continue to my account"};

            #endregion

        }
    }

    public class Element
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Content { get; set; }
    }
}
