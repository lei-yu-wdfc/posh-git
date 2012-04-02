using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    public class CollectionsCommunicationsChaseForDca
    {
        [FixtureSetUp]
        public static void FixtureSetUp()
        {
        }

        [FixtureTearDown]
        public static void FixtureTearDown()
        {
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenLoanTaggedAsMovedToDcaThenLoanShouldNotBeAddedToCollectionsQueueForCustomerSupport()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            //TODO: Verify loan has been removed from Collections Queue of Customer Support...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenLoanTaggedAsMovedToDcaThenLoanShouldHaveCollectionsChaseSuppressed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            //TODO: Verify Collections chase has been suppressed...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenDcaTagHasBeenRemovedByCsAgentThenLoanReEntersAgentQueue()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Drive.Cs.Commands.Post(new RevokeApplicationFromDcaCommand
            {
                ApplicationId = application.Id
            });

            //TODO: Verify loan has re-entered agents queue...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenDcaTagHasBeenRemovedByCsAgentThenLoanReEntersCollectionsChase()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            Drive.Cs.Commands.Post(new RevokeApplicationFromDcaCommand
            {
                ApplicationId = application.Id
            });

            //TODO: Verify loan has re-entered collections chase...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenDcaTagHasBeenRemovedAutomaticallyAfterSixMonthsThenLoanReEntersAgentQueue()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            //TODO: Wait 6 months?

            //TODO: Auto removed from dca?

            //TODO: Verify loan has re-entered agents queue...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenDcaTagHasBeenRemovedAutomaticallyAfterSixMonthsThenLoanReEntersCollectionsChase()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            application.MoveToDebtCollectionAgency();

            //TODO: Wait 6 months?

            //TODO: Auto removed from dca?

            //TODO: Verify loan has re-entered collections chase...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenCollectionsChaseHasCompletedThenOnlySixEmailsHaveBeenSent()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1810"), Ignore("Not fully implemented, do not run")]
        public void WhenCollectionsEmailsAreSentThenCorrectTemplateShouldBeUsed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
        }
    }
}
