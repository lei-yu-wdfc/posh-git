using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public static class CustomerOperations
    {

        public static readonly String CUSTOMER_FULL_NAME = "FULL_NAME";
        public static readonly String CUSTOMER_FULL_ADDRESS = "FULL_ADDRESS";
        private static readonly dynamic MarketingEligibleCustomersEntity = Drive.Data.Marketing.Db.MarketingEligibleCustomers;
        private static readonly dynamic RiskDb = Drive.Data.Risk.Db;

        #region Public Members
        /* Marketing Stuff */
        public static void CreateMarketingEligibility(Guid customerId, bool isEligible)
        {
            if (isEligible.Equals(true))
            {
                Do.Until(() => MarketingEligibleCustomersEntity.Insert(EligibleCustomerId: customerId, ShowAd: 1, CustomerInArrears: 0,
                                               CreateOn: Get.RandomDate(), HasStandardCard: 1, HasPremiumCard: 0));
            }
            else
            {
                Do.Until(() => MarketingEligibleCustomersEntity.Insert(EligibleCustomerId: customerId, ShowAd: 0, CustomerInArrears: 1,
                                              CreateOn: Get.RandomDate(), HasStandardCard: 1, HasPremiumCard: 0));
            }
        }
        public static void DeleteMarketingEligibility(Guid customerId)
        {
            Do.Until(() => MarketingEligibleCustomersEntity.Delete(EligibleCustomerId: customerId));
        }
        public static Dictionary<String, String> GetFullCustomerInfo(Guid customerId)
        {
            Dictionary<String, String> result = new Dictionary<string, string>();
            result.Add(CUSTOMER_FULL_NAME, GetFullCustomerName(customerId));
            result.Add(CUSTOMER_FULL_ADDRESS, GetFullCustomerAddress(customerId));
            return result;
        }
        public static void ChangeMarketingEligibility(Guid customerId, bool isEligible)
        {
            if (isEligible.Equals(true))
            {
                Do.Until(() => MarketingEligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId,
                                                                                   CustomerInArrears: 1));
            }
            else
            {
                Do.Until(() => MarketingEligibleCustomersEntity.UpdateByEligibleCustomerId(EligibleCustomerId: customerId,
                                                                                    CustomerInArrears: 0));
            }
        }

        /* Risk stuff */
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

        #endregion

        #region Private Members

        private static String GetFullCustomerName(Guid customerId)
        {
            StringBuilder builder = new StringBuilder();

            var request = new GetCustomerDetailsQuery();
            request.AccountId = customerId;

            var response = Drive.Api.Queries.Post(request);
            builder.Append(response.Values["Forename"].First());
            builder.Append(" ");
            builder.Append(response.Values["MiddleName"].First());
            builder.Append(" ");
            builder.Append(response.Values["Surname"].First());

            return builder.ToString();
        }
        private static String GetFullCustomerAddress(Guid customerId)
        {
            StringBuilder builder = new StringBuilder();

            var request = new GetCurrentAddressQuery();
            request.AccountId = customerId;

            var response = Drive.Api.Queries.Post(request);
            builder.Append(response.Values["HouseName"].First());
            builder.Append(response.Values["HouseNumber"].First());
            builder.Append(" ");
            builder.Append(response.Values["Street"].First());
            builder.Append("<br />");
            builder.Append(response.Values["Town"].First());
            builder.Append(" ");
            builder.Append(response.Values["Postcode"].First());

            return builder.ToString();
        }

        #endregion

        
    }
}
