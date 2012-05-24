using System;
using System.Globalization;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public static class CustomerOperations
    {
        private static readonly dynamic _eligibleCustomersEntity = Drive.Data.Marketing.Db.MarketingEligibleCustomers;
        private static readonly dynamic _commsDb = Drive.Data.Comms.Db;

        private static readonly String VERIFICATION_PIN = "0000";
        private static readonly String COUNTTRY_CODE = "UK";
        private static readonly String POST_CODE = "SW6 6PN";

        public static readonly String STANDARD_CARD_TYPE = "Standard";
        public static readonly String PREMIUM_CARD_TYPE = "Premium";
                      
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

        public static void UpdateCustomerPrepaidCard(Guid customerId,bool isPremiumCard)
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
        
        public static void ChangeMarketingEligibility(Guid customerId,bool isEligible)
        {
            if (isEligible.Equals(true))
            {
                Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId:customerId,
                                                                                   CustomerInArrears:1));
            }
            else
            {
                Do.Until(() => _eligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId:customerId,
                                                                                    CustomerInArrears:0));
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
            resendMobilePin.Pin = VERIFICATION_PIN;

            Drive.Api.Commands.Post(verificationMobileCommand);
            Drive.Api.Commands.Post(resendMobilePin);
        }

        public static void UpdateAddress(Guid customerId)
        {
            var customer = Do.Until(() => _commsDb.CustomerDetails.FindByAccountId(customerId));
            var address = Do.Until(() => _commsDb.Addresses.FindByAccountId(customerId));

            var command = new UpdateCustomerAddressUkCommand();
            command.AccountId = customerId;
            command.AddressId = address.ExternalId;
            command.AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date);
            command.CountryCode = COUNTTRY_CODE;
            command.Postcode = POST_CODE;
            command.HouseName = Get.RandomString(8);
            command.HouseNumber =  Get.RandomInt(1, 100).ToString(CultureInfo.InvariantCulture);
            command.District = Get.RandomString(15);
            command.Street = Get.RandomString(15);
            command.Town = Get.RandomString(15);
            command.County = Get.RandomString(15);
                                  

            Drive.Api.Commands.Post(command);
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
        }

        public static void CreatePrepaidCardForCustomer(Guid customerId,bool isPremiumCard)
        {
            if(isPremiumCard.Equals(true))
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
        }

        public static void SetFundsForCustomer(Guid applicationId,bool isPrepaidFunds)
        {
            var request = new SetFundsTransferMethodCommand();
            request.ApplicationId = applicationId;

            if(isPrepaidFunds.Equals(true))
            {
                request.TransferMethod = FundsTransferEnum.SendToPrepaidCard;   
            }
            else
            {
                request.TransferMethod = FundsTransferEnum.DefaultAutomaticallyChosen;
            }

            Drive.Api.Commands.Post(request);
        }
    }
}
