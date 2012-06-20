using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture, AUT(AUT.Wb)]
    public class SalesforceActivityTests: SalesforceTestBase
    {
        private Customer customer;
        private Organisation organisation;
        private Application application;

        [FixtureSetUp]
        public void InitFixture()
        {
            customer = CustomerBuilder.New().Build();
            organisation = OrganisationBuilder.New(customer).Build();
            application = ApplicationBuilder.New(customer, organisation).Build();
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1411"), Explicit("Slow TC does not cope with the load of all acceptance tests being run simultaneously")]
        public void Salesforce_ShouldCreateActivityForContact_WhenRecordActivityCommandIsSent()
        {
            var recordActivityCommand = new RecordActivityCommand
                                            {
                                                AccountId = customer.Id, ActivityType = Guid.NewGuid().ToString(), Subject = Guid.NewGuid().ToString()
                                            };
            Drive.Msmq.Salesforce.Send(recordActivityCommand);

            var salesforceContactId = Do.Until(() => (string)Drive.Data.Salesforce.Db.SalesforceAccounts.FindByAccountIdAndType(customer.Id, 1).SalesforceId);

            Do.Until(() => Salesforce.GetTask(salesforceContactId, "WhoId", recordActivityCommand.ActivityType,
                                   recordActivityCommand.Subject));
        }
    }
}
