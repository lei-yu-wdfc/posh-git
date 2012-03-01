using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.PaymentFailureEmailTests
{
    [TestFixture, AUT(AUT.Wb)]    
    class PaymentFailureEmails
    {
        private static Guid orgId = Guid.Empty;
        private BusinessApplication applicationInfo;
        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            orgId = organization.Id;
            applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build() as BusinessApplication;
        }

        [Test, JIRA("SME-810")]
        public void EmailIsSentToCustomerOnCollectionFailure()
        {
            applicationInfo.GetPaymentPlan();
            applicationInfo.FirstCollectionAttempt(null, false, false);
            applicationInfo.SecondCollectionAttempt(false);
                        
            Do.Until(() => Driver.Db.OpsSagasWb.FirstPaymentRequestFailedSagaEntities.Single(t => t.ApplicationId == applicationInfo.Id
                                                                                                   && t.EmailSent == true));
            Do.Until(() => Driver.Db.OpsSagasWb.SecondPaymentRequestFailedSagaEntities.Single(t => t.ApplicationId == applicationInfo.Id
                                                                                                   && t.EmailSent == true));
        }
    }
}
