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
    }
}
