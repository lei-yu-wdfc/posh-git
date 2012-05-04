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

        private static readonly dynamic _eligibleCustomersEntity = Drive.Data.Marketing.Db.MarketingEligibleCustomers;

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


        public static void DeleteMarketingEligibility(Guid customerId)
        {
            Do.Until(() => _eligibleCustomersEntity.Delete(EligibleCustomerId: customerId));
        }

        public static Dictionary<String, String> GetFullCustomerInfo(Guid customerId)
        {
            Dictionary<String, String> result = new Dictionary<string, string>();
            result.Add(CUSTOMER_FULL_NAME, GetFullCustomerName(customerId));
            result.Add(CUSTOMER_FULL_ADDRESS, GetFullCustomerAddress(customerId));
            return result;
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
    }
}
