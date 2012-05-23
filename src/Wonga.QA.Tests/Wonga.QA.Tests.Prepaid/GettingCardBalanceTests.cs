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
        [Test, AUT(AUT.Uk), JIRA("PP-203")]
        public void GettingAvailableBalanceFromDatabaseTest()
        {
            var query = new GetPrepaidAvailableAccountBalanceQuery()
            {
                CustomerExternalId = new Guid("5b247b31-2e31-4625-a04b-2373054e5a57")
            };
            var firstResponse = Drive.Api.Queries.Post(query);

            var CachedAccountBalances = Drive.Data.PPS.Db.CachedAccountBalances;
            var allRecords = Do.Until(() => CachedAccountBalances.FindAll(CachedAccountBalances.Id > 0));
            var firstTimeRecordsCount = allRecords.Count();

            var secondResponce = Drive.Api.Queries.Post(query);

            allRecords = Do.Until(() => CachedAccountBalances.FindAll(CachedAccountBalances.Id > 0));
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
                CustomerExternalId = new Guid("5b247b31-2e31-4625-a04b-2373054e5a57")
            };
            var firstResponse = Drive.Api.Queries.Post(query);

            var CachedAccountBalances = Drive.Data.PPS.Db.CachedAccountBalances;
            var allRecords = Do.Until(() => CachedAccountBalances.FindAll(CachedAccountBalances.Id > 0));
            var firstTimeRecordsCount = allRecords.Count();

            var ServiceConfigurations = Drive.Data.Ops.Db.ServiceConfigurations;
            var confRecord = Do.Until(() => ServiceConfigurations.Find(ServiceConfigurations.Key == configurationKey ));
            var waitValue = confRecord.Value;
            Do.Until(() => ServiceConfigurations.UpdateByKey(
                     Key: configurationKey,
                     Value: 1));

            Thread.Sleep(60*1000+100);

            var secondResponce = Drive.Api.Queries.Post(query);

            Do.Until(() => ServiceConfigurations.UpdateByKey(
                     Key: configurationKey,
                     Value: waitValue));

            allRecords = Do.Until(() => CachedAccountBalances.FindAll(CachedAccountBalances.Id > 0));
            var secondTimeRecordsCount = allRecords.Count();

            Assert.AreNotEqual(firstTimeRecordsCount, secondTimeRecordsCount);
        }
    }
}
