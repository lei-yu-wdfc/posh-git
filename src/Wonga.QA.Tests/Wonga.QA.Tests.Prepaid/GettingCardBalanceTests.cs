using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Prepaid
{
    public class GettingCardBalanceTests
    {
        private Customer _eligibleCustomer;
        private static readonly dynamic _cachedAccountBalancesEntity = Drive.Data.PPS.Db.CachedAccountBalances;
        private static readonly dynamic _serviceConfigurationsEntity = Drive.Data.Ops.Db.ServiceConfigurations;

        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id, true);
            CustomerOperations.CreatePrepaidCardForCustomer(_eligibleCustomer.Id, false);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-203")]
        public void GettingAvailableBalanceFromDatabaseTest()
        {
            var query = new GetPrepaidAvailableAccountBalanceQuery()
            {
                CustomerExternalId = _eligibleCustomer.Id
            };
            var firstResponse = Drive.Api.Queries.Post(query);

            var allRecords = Do.Until(() => _cachedAccountBalancesEntity.FindAll(_cachedAccountBalancesEntity.Id > 0));
            var firstTimeRecordsCount = allRecords.Count();

            var secondResponce = Drive.Api.Queries.Post(query);

            allRecords = Do.Until(() => _cachedAccountBalancesEntity.FindAll(_cachedAccountBalancesEntity.Id > 0));
            var secondTimeRecordsCount = allRecords.Count();
            
            Assert.AreEqual(firstResponse.Values["Balance"].Single(), secondResponce.Values["Balance"].Single());
            Assert.AreEqual(firstTimeRecordsCount, secondTimeRecordsCount);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-203")]
        public void GettingAvailableBalanceFromPPSTest()
        {
            const String configurationKey = "PrepaidCard.TIME_OF_ACTUALLY_CACHED_ACCOUNT_BALANCE_DATA_IN_MINUTES";

            var query = new GetPrepaidAvailableAccountBalanceQuery()
            {
                CustomerExternalId = _eligibleCustomer.Id
            };
            var firstResponse = Drive.Api.Queries.Post(query);

            var allRecords = Do.Until(() => _cachedAccountBalancesEntity.FindAll(_cachedAccountBalancesEntity.Id > 0));
            var firstTimeRecordsCount = allRecords.Count();


            var confRecord = Do.Until(() => _serviceConfigurationsEntity.Find(_serviceConfigurationsEntity.Key == configurationKey));
            var waitValue = confRecord.Value;
            Do.Until(() => _serviceConfigurationsEntity.UpdateByKey(
                     Key: configurationKey,
                     Value: 1));

            Thread.Sleep(60*1000+100);

            var secondResponce = Drive.Api.Queries.Post(query);

            Do.Until(() => _serviceConfigurationsEntity.UpdateByKey(
                     Key: configurationKey,
                     Value: waitValue));

            allRecords = Do.Until(() => _cachedAccountBalancesEntity.FindAll(_cachedAccountBalancesEntity.Id > 0));
            var secondTimeRecordsCount = allRecords.Count();

            Assert.AreNotEqual(firstTimeRecordsCount, secondTimeRecordsCount);
        }
    }
}
