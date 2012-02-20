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

            SliderElement = new SliderElement
            {
                FormId = XmlMapper.GetValue(() => SliderElement.FormId),
                LoanAmount = XmlMapper.GetValue(() => SliderElement.LoanAmount),
                LoanDuration = XmlMapper.GetValue(() => SliderElement.LoanDuration),
                SubmitButton = XmlMapper.GetValue(() => SliderElement.SubmitButton),
            };

            YourNameElement = new YourNameElement
                                  {
                                      Legend = XmlMapper.GetValue(()=>YourNameElement.Legend),
                                      Title = XmlMapper.GetValue(() => YourNameElement.Title),
                                      FirstName = XmlMapper.GetValue(() => YourNameElement.FirstName),
                                      MiddleName = XmlMapper.GetValue(() => YourNameElement.MiddleName),
                                      LastName = XmlMapper.GetValue(() => YourNameElement.LastName),
                                  };

            YourDetailsElement = new YourDetailsElement
                                     {
                                         Legend = XmlMapper.GetValue(() => YourDetailsElement.Legend),
                                         Dependants = XmlMapper.GetValue(() => YourDetailsElement.Dependants),
                                         Gender = XmlMapper.GetValue(() => YourDetailsElement.Gender),
                                         DateOfBirthDay = XmlMapper.GetValue(() => YourDetailsElement.DateOfBirthDay),
                                         DateOfBirthMonth = XmlMapper.GetValue(() => YourDetailsElement.DateOfBirthMonth),
                                         DateOfBirthYear = XmlMapper.GetValue(() => YourDetailsElement.DateOfBirthYear),
                                         HomeStatus = XmlMapper.GetValue(() => YourDetailsElement.HomeStatus),
                                         MaritalStatus = XmlMapper.GetValue(() => YourDetailsElement.MaritalStatus),
                                     };

            MobilePinVerificationElement = new MobilePinVerificationElement { Pin = XmlMapper.GetValue(() => MobilePinVerificationElement.Pin), Legend = XmlMapper.GetValue(() => MobilePinVerificationElement.Legend) };

            ContactingYouElement = new ContactingYouElement
                                       {
                                           Legend = XmlMapper.GetValue(() => ContactingYouElement.Legend),
                                           Email = XmlMapper.GetValue(() => ContactingYouElement.Email),
                                           EmailConfirm = XmlMapper.GetValue(() => ContactingYouElement.EmailConfirm),
                                           HomePhone = XmlMapper.GetValue(() => ContactingYouElement.HomePhone),
                                           MobilePhone = XmlMapper.GetValue(() => ContactingYouElement.MobilePhone),
                                       };

            DebitCardElement = new DebitCardElement
                                          {
                                              Legend = XmlMapper.GetValue(() => DebitCardElement.Legend),
                                              CardType = XmlMapper.GetValue(() => DebitCardElement.CardType),
                                              CardNumber = XmlMapper.GetValue(() => DebitCardElement.CardNumber),
                                              CardName = XmlMapper.GetValue(() => DebitCardElement.CardName),
                                              CardExpiryDateMonth = XmlMapper.GetValue(() => DebitCardElement.CardExpiryDateMonth),
                                              CardExpiryDateYear = XmlMapper.GetValue(() => DebitCardElement.CardExpiryDateYear),
                                              CardSecurityNumber = XmlMapper.GetValue(() => DebitCardElement.CardSecurityNumber),
                                              CardStartDateMonth = XmlMapper.GetValue(() => DebitCardElement.CardStartDateMonth),
                                              CardStartDateYear = XmlMapper.GetValue(() => DebitCardElement.CardStartDateYear),
                                          };

            BankAccountElement = new BankAccountElement
                                            {
                                                Legend = XmlMapper.GetValue(() => BankAccountElement.Legend),
                                                BankName = XmlMapper.GetValue(() => BankAccountElement.BankName),
                                                SortCodePart1 = XmlMapper.GetValue(() => BankAccountElement.SortCodePart1),
                                                SortCodePart2 = XmlMapper.GetValue(() => BankAccountElement.SortCodePart2),
                                                SortCodePart3 = XmlMapper.GetValue(() => BankAccountElement.SortCodePart3),
                                                AccountNumber = XmlMapper.GetValue(() => BankAccountElement.AccountNumber),
                                                BankPeriod = XmlMapper.GetValue(() => BankAccountElement.BankPeriod),
                                            };

            

            AccountDetailsElement = new AccountDetailsElement
                                        {
                                            Legend = XmlMapper.GetValue(() => AccountDetailsElement.Legend),
                                            Password = XmlMapper.GetValue(() => AccountDetailsElement.Password),
                                            PasswordConfirm = XmlMapper.GetValue(() => AccountDetailsElement.PasswordConfirm),
                                            SecretQuestion = XmlMapper.GetValue(() => AccountDetailsElement.SecretQuestion),
                                            SecretAnswer = XmlMapper.GetValue(() => AccountDetailsElement.SecretAnswer),
                                        };

            #endregion

            #region WbPagesMappings

            WbEligibilityQuestionsPage = new EligibilityQuestionsPage
                                             {
                                                 FormId = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.FormId),
                                                 CheckDirector = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckDirector),
                                                 CheckResident = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckResident),
                                                 CheckActiveCompany = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckActiveCompany),
                                                 CheckTurnover = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckTurnover),
                                                 CheckVat = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckVat),
                                                 CheckOnlineAccess = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckOnlineAccess),
                                                 CheckGuarantee = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckGuarantee),
                                                 NextButton = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.NextButton),
                                                 CheckDebitCard = XmlMapper.GetValue(() => WbEligibilityQuestionsPage.CheckDebitCard),
                                             };

            PersonalDetailsPage = new PersonalDetailsPage
                                      {
                                          FormId = XmlMapper.GetValue(() => PersonalDetailsPage.FormId),
                                          CheckPrivacyPolicy = XmlMapper.GetValue(() => PersonalDetailsPage.CheckPrivacyPolicy),
                                          CheckCanContact = XmlMapper.GetValue(() => PersonalDetailsPage.CheckCanContact),
                                          NextButton = XmlMapper.GetValue(() => PersonalDetailsPage.NextButton),
                                      };

            AddressDetailsPage = new AddressDetailsPage
                                       {
                                           FormId = XmlMapper.GetValue(() => AddressDetailsPage.FormId),
                                           LookupButton = XmlMapper.GetValue(() => AddressDetailsPage.LookupButton),
                                           PostCode = XmlMapper.GetValue(() => AddressDetailsPage.PostCode),
                                           District = XmlMapper.GetValue(() => AddressDetailsPage.District),
                                           FlatNumber = XmlMapper.GetValue(() => AddressDetailsPage.FlatNumber),
                                           County = XmlMapper.GetValue(() => AddressDetailsPage.County),
                                           AddressPeriod = XmlMapper.GetValue(() => AddressDetailsPage.AddressPeriod),
                                           NextButton = XmlMapper.GetValue(() => AddressDetailsPage.NextButton),
                                           AddressOptions = XmlMapper.GetValue(() => AddressDetailsPage.AddressOptions),
                                       };

            AccountDetailsPage = new AccountDetailsPage { FormId = XmlMapper.GetValue(() => AccountDetailsPage.FormId), NextButton = XmlMapper.GetValue(() => AccountDetailsPage.NextButton), };

            WbPersonalBankAccountPage = new PersonalBankAccountDetailsPage { FormId = XmlMapper.GetValue(() => WbPersonalBankAccountPage.FormId), NextButton = XmlMapper.GetValue(() => WbPersonalBankAccountPage.NextButton), };

            WbPersonalDebitCardDetailsPage = new PersonalDebitCardPage { FormId = XmlMapper.GetValue(() => WbPersonalDebitCardDetailsPage.FormId), NextButton = XmlMapper.GetValue(() => WbPersonalDebitCardDetailsPage.NextButton), };

            WbBusinessDetailsPage = new BusinessDetailsPage
                                        {
                                            FormId = XmlMapper.GetValue(() => WbBusinessDetailsPage.FormId),
                                            NextButton = XmlMapper.GetValue(() => WbBusinessDetailsPage.NextButton),
                                            BusinessNumber = XmlMapper.GetValue(() => WbBusinessDetailsPage.BusinessNumber),
                                            BusinessName = XmlMapper.GetValue(() => WbBusinessDetailsPage.BusinessName),
                                        };

            WbAdditionalDirectorsPage = new AdditionalDirectorsPage
                                            {
                                                FormId = XmlMapper.GetValue(() => WbAdditionalDirectorsPage.FormId),
                                                DoneButton = XmlMapper.GetValue(() => WbAdditionalDirectorsPage.DoneButton),
                                                AddAnotherDirector = XmlMapper.GetValue(() => WbAdditionalDirectorsPage.AddAnotherDirector),
                                            };

            WbAddAditionalDirectorsPage = new AddAditionalDirectorsPage
                                              {
                                                  FormId = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.FormId),
                                                  Title = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.Title),
                                                  ConfirmEmailAddress = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.ConfirmEmailAddress),
                                                  DoneButton = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.DoneButton),
                                                  EmailAddress = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.EmailAddress),
                                                  FirstName = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.FirstName),
                                                  LastName = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.LastName),
                                                  AddAnotherButton = XmlMapper.GetValue(() => WbAddAditionalDirectorsPage.AddAnotherButton),
                                              };

            BankAccountPage = new BankAccountPage { FormId = XmlMapper.GetValue(() => BankAccountPage.FormId), NextButton = XmlMapper.GetValue(() => BankAccountPage.NextButton), };

            WbBusinessDebitCardPage = new BusinessDebitCardPage { FormId = XmlMapper.GetValue(() => WbBusinessDebitCardPage.FormId), NextButton = XmlMapper.GetValue(() => WbBusinessDebitCardPage.NextButton), };

            ProcessingPage = new ProcessingPage
                                 {
                                     ProcessingText = XmlMapper.GetValue(() => ProcessingPage.ProcessingText),
                                     ProcessingTextContainer = XmlMapper.GetValue(() => ProcessingPage.ProcessingTextContainer),
                                 };

            WbAcceptedPage = new AcceptedPage
                                 {
                                     FormId = XmlMapper.GetValue(() => WbAcceptedPage.FormId),
                                     AcceptBusinessLoan = XmlMapper.GetValue(() => WbAcceptedPage.AcceptBusinessLoan),
                                     AcceptGuarantorLoan = XmlMapper.GetValue(() => WbAcceptedPage.AcceptGuarantorLoan),
                                     SubmitButton = XmlMapper.GetValue(() => WbAcceptedPage.SubmitButton),
                                 };

            DealDonePage = new DealDonePage { HeaderText = XmlMapper.GetValue(() => DealDonePage.HeaderText), ContinueButtonLinkText = XmlMapper.GetValue(() => DealDonePage.ContinueButtonLinkText), };

            DeclinedPage = new DeclinedPage { HeaderText = XmlMapper.GetValue(() => DeclinedPage.HeaderText), };

            #endregion

        }
    }
}
