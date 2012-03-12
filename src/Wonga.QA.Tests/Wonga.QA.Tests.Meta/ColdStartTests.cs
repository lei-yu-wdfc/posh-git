using System;
using System.Net;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Svc;

namespace Wonga.QA.Tests.Meta
{
    [Parallelizable(TestScope.All)]
    public class ColdStartTests
    {
        private ApiEndpoint[] Endpoints()
        {
            ApiDriver api = Driver.Api;
            return new[]
            {
                api.Commands,
                api.Queries
            };
        }

        private DbDatabase.Box[] Databases()
        {
            DbDriver db = Driver.Db;
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
            SvcDriver svc = Driver.Svc;
            return new[]
            {
                svc.Ops,
                svc.Comms,
                svc.Payments,
                svc.Risk
            };
        }

        [Test, Factory("Endpoints")]
        public void EndpointIsWarmedUpAndSchemaIsValid(ApiEndpoint endpoint)
        {
            Assert.DoesNotThrow(() => endpoint.GetShema());
        }

        [Test, Factory("Databases")]
        public void DatabaseConnectionCanBeOpened(DbDatabase.Box database)
        {
            Assert.IsTrue(database.Exists());
        }

        [Test, Factory("Services")]
        public void ServiceIsStarted(SvcService service)
        {
            Assert.IsTrue(service.IsRunning());
        }

        [Test, DependsOn("EndpointIsWarmedUpAndSchemaIsValid")]
        public void HomePageCanBeLoaded()
        {
            Assert.Contains(new WebClient().DownloadString(Config.Ui.Home), "Wonga");
        }
    }
}
