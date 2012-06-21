using System;
using System.Globalization;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.PrepaidCard.Commands;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public static class CustomerOperations
    {
        private static readonly dynamic _eligibleCustomersEntity = Drive.Data.Marketing.Db.MarketingEligibleCustomers;
        private static readonly dynamic _commsDb = Drive.Data.Comms.Db;
        private static readonly dynamic _prepaidDb = Drive.Data.PrepaidCard.Db;
        private static readonly dynamic RiskDb = Drive.Data.Risk.Db;

        public static void CreateMarketingEligibility(Guid customerId, bool isEligible)
        {
            if (isEligible.Equals(true))
            {
                Do.Until(() => _eligibleCustomersEntity.Insert(EligibleCustomerId: customerId, ShowAd: 1, CustomerInArrears: 0,
                                               CreateOn: Get.RandomDate(), HasStandardCard: 1, HasPremiumCard: 0));
            }
            else
            {
                Do.Until(() => _eligibleCustomersEntity.Insert(EligibleCustomerId: customerId, ShowAd: 0, CustomerInArrears: 1,
                                              CreateOn: Get.RandomDate(), HasStandardCard: 1, HasPremiumCard: 0));
            }
        }

        public static void MakeZeroCardsForCustomer(Guid customerId)
        {
            Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId, HasStandardCard: 0, HasPremiumCard: 0));
        }
        public static void UpdateCustomerPrepaidCard(Guid customerId, bool isPremiumCard)
        {
            if (isPremiumCard.Equals(true))
            {
                Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId, HasStandardCard: 0, HasPremiumCard: 1));
            }
            else
            {
                Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId, HasStandardCard: 1, HasPremiumCard: 0));
            }
        }

        public static void DeleteMarketingEligibility(Guid customerId)
        {
            Do.Until(() => _eligibleCustomersEntity.Delete(EligibleCustomerId: customerId));
        }

        public static void ChangeMarketingEligibility(Guid customerId, bool isEligible)
        {
            if (isEligible.Equals(true))
            {
                Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId,
                                                                                   CustomerInArrears: 1));
            }
            else
            {
                Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId,
                                                                                    CustomerInArrears: 0));
            }
        }

        public static void UpdateMobilePhone(Guid customerId)
        {
            var customer = Do.Until(() => _commsDb.CustomerDetails.FindByAccountId(customerId));

            var verificationMobileCommand = new VerifyMobilePhoneUkCommand();
            verificationMobileCommand.AccountId = customerId;
            verificationMobileCommand.Forename = customer.Forename;
            verificationMobileCommand.MobilePhone = Get.GetMobilePhone();
            verificationMobileCommand.VerificationId = Guid.NewGuid();

            var resendMobilePin = new CompleteMobilePhoneVerificationCommand();
            resendMobilePin.VerificationId = verificationMobileCommand.VerificationId;
            resendMobilePin.Pin = Get.GetVerificationPin();

            Drive.Api.Commands.Post(verificationMobileCommand);
            Drive.Api.Commands.Post(resendMobilePin);

            Do.Until(() => _commsDb.CustomerDetails.FindBy(AccountId: customerId, MobilePhone: verificationMobileCommand.MobilePhone));

        }
        public static void UpdateAddress(Guid customerId)
        {
            var customer = Do.Until(() => _commsDb.CustomerDetails.FindByAccountId(customerId));
            var address = Do.Until(() => _commsDb.Addresses.FindByAccountId(customerId));

            var command = new UpdateCustomerAddressUkCommand();
            command.AccountId = customerId;
            command.AddressId = address.ExternalId;
            command.AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
            command.Postcode = Get.GetPostcode();
            command.CountryCode = Get.GetCountryCode();
            command.HouseName = Get.RandomString(8);
            command.HouseNumber = Get.RandomInt(1, 100).ToString(CultureInfo.InvariantCulture);
            command.District = Get.RandomString(15);
            command.Street = Get.RandomString(15);
            command.Town = Get.RandomString(15);
            command.County = Get.RandomString(15);

            Drive.Api.Commands.Post(command);
            Do.Until(() => _commsDb.Addresses.FindBy(AccountId: customerId, Street: command.Street, Town: command.Town));
        }

        public static void UpdateEmail(Guid customerId)
        {
            var customer = Do.Until(() => _commsDb.CustomerDetails.FindByAccountId(customerId));

            var verificationEmail = new SendVerificationEmailCommand();
            verificationEmail.AccountId = customerId;
            verificationEmail.Email = Get.GetEmail(50);
            verificationEmail.UriFragment = Config.Ui.Home + "confirm-new-email-address/";

            Drive.Api.Commands.Post(verificationEmail);

            var emailVerification = Do.Until(() => _commsDb.EmailVerification.FindByAccountId(customerId));

            var completeEmailVerification = new CompleteEmailVerificationCommand();
            completeEmailVerification.AccountId = customerId;
            completeEmailVerification.ChangeId = emailVerification.ChangeId;

            Drive.Api.Commands.Post(completeEmailVerification);
            Do.Until(() => _commsDb.CustomerDetails.FindBy(AccountId: customerId, Email: verificationEmail.Email));
        }

        public static void CreatePrepaidCardForCustomer(Guid customerId, bool isPremiumCard)
        {
            if (isPremiumCard.Equals(true))
            {
                var request = new SignupCustomerForPremiumCardCommand();
                request.CustomerExternalId = customerId;
                Drive.Api.Commands.Post(request);
            }

            else
            {
                var request = new SignupCustomerForStandardCardCommand();
                request.CustomerExternalId = customerId;
                Drive.Api.Commands.Post(request);
            }
            var cardHolder = Do.Until(() => _prepaidDb.CardHolderDetails.FindByCustomerExternalId(customerId));
            Do.Until(() => _prepaidDb.CardDetails.FindByCardHolderExternalId(cardHolder.ExternalId));
        }

        public static void SetFundsForCustomer(Guid applicationId, bool isPrepaidFunds)
        {
            var request = new SetFundsTransferMethodCommand();
            request.ApplicationId = applicationId;

            if (isPrepaidFunds.Equals(true))
            {
                request.TransferMethod = FundsTransferMethodEnum.SendToPrepaidCard;
            }
            else
            {
                request.TransferMethod = FundsTransferMethodEnum.DefaultAutomaticallyChosen;
            }

            Drive.Api.Commands.Post(request);
        }

        public static void UpdateEmployerNameInRisk(Guid accountId, string employerName)
        {
            Do.Until(() => RiskDb.EmploymentDetails.UpdateByAccountId(AccountId: accountId, EmployerName: employerName));
        }

        public static void UpdateMiddleNameInRisk(Guid accountId, string middleName)
        {
            Do.Until(() => RiskDb.RiskAccounts.UpdateByAccountId(AccountId: accountId, MiddleName: middleName));
        }

        public static void RemovePhoneNumberFromRisk(string phoneNumber)
        {
            Do.Until(() => RiskDb.RiskAccountMobilePhones.DeleteAllByMobilePhone(phoneNumber));
        }

        public static void AddPhoneNumberToRisk(string mobilePhoneNumber)
        {
            var tempGuid = Guid.NewGuid();

            //Insert a new RiskAccount
            Do.Until(
                () =>
                RiskDb.RiskAccounts.Insert(AccountId: tempGuid, AccountRank: 1, HasAccount: true, CreditLimit: 400,
                                           ConfirmedFraud: false, DateOfBirth: Get.GetDoB(), DoNotRelend: false,
                                           Forename: Get.RandomString(8), IsDebtSale: false, IsDispute: false,
                                           IsHardship: false, Postcode: "KT2 5DL", MaidenName: Get.RandomString(8),
                                           Middlename: Get.RandomString(8), Surname: Get.RandomString(8)));

            //Insert a new RiskAccountMobilePhone
            Do.Until(
                () =>
                RiskDb.RiskAccountMobilePhones.Insert(AccountId: tempGuid,
                                                      DateUpdated: new DateTime(2010, 10, 06).ToDate(),
                                                      MobilePhone: mobilePhoneNumber));

        }



    }
}