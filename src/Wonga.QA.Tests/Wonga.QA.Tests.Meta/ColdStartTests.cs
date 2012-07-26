using System;
using System.Net;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Svc;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Meta
{
    [Parallelizable(TestScope.All), Category(TestCategories.CoreTest)]
    public class ColdStartTests
    {
        private ApiEndpoint[] Endpoints()
        {
            ApiDriver api = Drive.Api;
            return new[]
            {
                api.Commands,
                api.Queries
            };
        }

        private DbDatabase.Box[] Databases()
        {
            DbDriver db = Drive.Db;
            return new[]
            {
                db.Ops.Boxed,
                db.Comms.Boxed,
                db.Payments.Boxed,
                db.Risk.Boxed
            };
        }

        private SvcService[] Services()
        {
            SvcDriver svc = Drive.Svc;
            return new[]
            {
                svc.Ops,
                svc.Comms,
                svc.Payments,
                svc.Risk
            };
        }

        [Test, Factory("Endpoints"), Owner(Owner.KonstantinosKonstantinidis)]
        public void ApiEndpointIsAvailable(ApiEndpoint endpoint)
        {
            Do.With.Timeout(5).Until(() =>
            {
                try
                {
                    endpoint.Get();
                }
                catch (WebException exception)
                {
                    if (exception.Status == WebExceptionStatus.Timeout)
                        throw;
                }
                return true;
            });
        }

        [Test, DependsOn("ApiEndpointIsAvailable"), Factory("Endpoints"), Owner(Owner.KonstantinosKonstantinidis)]
        public void ApiEndpointSchemaIsValid(ApiEndpoint endpoint)
        {
            Assert.DoesNotThrow(() => endpoint.GetShema());
        }

        [Test, Factory("Databases"), Owner(Owner.IlyaKozhevnikov)]
        public void DatabaseConnectionCanBeOpened(DbDatabase.Box database)
        {
            Assert.IsTrue(database.Exists());
        }

        [Test, Factory("Services"), Owner(Owner.IlyaKozhevnikov)]
        public void ServiceIsStarted(SvcService service)
        {
            Assert.IsTrue(service.IsRunning());
        }

        [Test, DependsOn("ApiEndpointIsAvailable"), DependsOn("ApiEndpointSchemaIsValid"), SUT(SUT.WIP, SUT.RC, SUT.WIPRelease, SUT.RCRelease, SUT.UAT, SUT.Live), Owner(Owner.IlyaKozhevnikov)]
        public void HomePageCanBeLoaded()
        {
            Assert.Contains(new WebClient().DownloadString(Config.Ui.Home), "Wonga");
        }
    }
}
