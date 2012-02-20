using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Mappings.Pages;
using Wonga.QA.Framework.UI.Mappings.Sections;

namespace Wonga.QA.Framework.UI.Mappings
{
    internal sealed class UkElements : BaseElements
    {
        internal UkElements()
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
                Legend = XmlMapper.GetValue(() => YourNameElement.Legend),
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

            EmploymentDetailsElement = new EmploymentDetailsElement
                                           {
                                               Legend = XmlMapper.GetValue(() => EmploymentDetailsElement.Legend),
                                               EmploymentStatus = XmlMapper.GetValue(() => EmploymentDetailsElement.EmploymentStatus),
                                               EmployerIndustry = XmlMapper.GetValue(() => EmploymentDetailsElement.EmployerIndustry),
                                               EmployerName = XmlMapper.GetValue(() => EmploymentDetailsElement.EmployerName),
                                               EmploymentPosition = XmlMapper.GetValue(() => EmploymentDetailsElement.EmploymentPosition),
                                               MonthlyIncome = XmlMapper.GetValue(() => EmploymentDetailsElement.MonthlyIncome),
                                               NextPaydayDay = XmlMapper.GetValue(() => EmploymentDetailsElement.NextPaydayDay),
                                               NextPaydayMonth = XmlMapper.GetValue(() => EmploymentDetailsElement.NextPaydayMonth),
                                               NextPaydayYear = XmlMapper.GetValue(() => EmploymentDetailsElement.NextPaydayYear),
                                               SalaryPaidToBank = XmlMapper.GetValue(() => EmploymentDetailsElement.SalaryPaidToBank),
                                               TimeWithEmployerMonths = XmlMapper.GetValue(() => EmploymentDetailsElement.TimeWithEmployerMonths),
                                               TimeWithEmployerYears = XmlMapper.GetValue(() => EmploymentDetailsElement.TimeWithEmployerYears),
                                               WorkPhone = XmlMapper.GetValue(() => EmploymentDetailsElement.WorkPhone),
                                           };

            ContactingYouElement = new ContactingYouElement
                                       {
                                           Legend = XmlMapper.GetValue(() => ContactingYouElement.Legend),
                                           HomePhone = XmlMapper.GetValue(() => ContactingYouElement.HomePhone),
                                           MobilePhone = XmlMapper.GetValue(() => ContactingYouElement.MobilePhone),
                                           Email = XmlMapper.GetValue(() => ContactingYouElement.Email),
                                           EmailConfirm = XmlMapper.GetValue(() => ContactingYouElement.EmailConfirm),
                                       };

            AccountDetailsElement = new AccountDetailsElement
                                        {
                                            Legend = XmlMapper.GetValue(() => AccountDetailsElement.Legend),
                                            Password = XmlMapper.GetValue(() => AccountDetailsElement.Password),
                                            PasswordConfirm = XmlMapper.GetValue(() => AccountDetailsElement.PasswordConfirm),
                                            SecretQuestion = XmlMapper.GetValue(() => AccountDetailsElement.SecretQuestion),
                                            SecretAnswer = XmlMapper.GetValue(() => AccountDetailsElement.SecretAnswer),
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

            MobilePinVerificationElement = new MobilePinVerificationElement { Pin = XmlMapper.GetValue(() => MobilePinVerificationElement.Pin), Legend = XmlMapper.GetValue(() => MobilePinVerificationElement.Legend) };


            #endregion

            #region UkPagesMappings

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

            PersonalDetailsPage = new PersonalDetailsPage
                                      {
                                          FormId = XmlMapper.GetValue(() => PersonalDetailsPage.FormId),
                                          CheckPrivacyPolicy = XmlMapper.GetValue(() => PersonalDetailsPage.CheckPrivacyPolicy),
                                          CheckCanContact = XmlMapper.GetValue(() => PersonalDetailsPage.CheckCanContact),
                                          NextButton = XmlMapper.GetValue(() => PersonalDetailsPage.NextButton),
                                      };

            AccountDetailsPage = new AccountDetailsPage { FormId = XmlMapper.GetValue(() => AccountDetailsPage.FormId), NextButton = XmlMapper.GetValue(() => AccountDetailsPage.NextButton), };

            BankAccountPage = new BankAccountPage { FormId = XmlMapper.GetValue(() => BankAccountPage.FormId), NextButton = XmlMapper.GetValue(() => BankAccountPage.NextButton), };

            DebitCardPage = new DebitCardPage
                                {
                                    FormId = XmlMapper.GetValue(() => DebitCardPage.FormId),
                                    NextButton = XmlMapper.GetValue(() => DebitCardPage.NextButton),
                                };

            ProcessingPage = new ProcessingPage
                                 {
                                     ProcessingTextContainer = XmlMapper.GetValue(() => ProcessingPage.ProcessingTextContainer),
                                     ProcessingText = XmlMapper.GetValue(() => ProcessingPage.ProcessingText),
                                 };

            DeclinedPage = new DeclinedPage { HeaderText = XmlMapper.GetValue(() => DeclinedPage.HeaderText), };

            #endregion
        }
    }
}
