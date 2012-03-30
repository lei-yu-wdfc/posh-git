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
        private BusinessApplication _applicationInfo;

        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().WithEmailAddress("qa.wb.wonga.com+dsjhsd@gmail.com").Build();
            var organization = OrganisationBuilder.New(customer).Build();
            _applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build() as BusinessApplication;
        }

        [Test, JIRA("SME-810")]
        public void EmailIsSentToCustomerOnCollectionFailure()
        {
            Assert.IsNotNull(_applicationInfo.GetPaymentPlan());
            _applicationInfo.FirstCollectionAttempt(null, false, false);
            _applicationInfo.SecondCollectionAttempt(false);

            Do.Until(() => Drive.Db.OpsSagasWb.FirstPaymentRequestFailedSagaEntities.Single(t => t.ApplicationId == _applicationInfo.Id
                                                                                                   && t.EmailSent == true));
            Do.Until(() => Drive.Db.OpsSagasWb.SecondPaymentRequestFailedSagaEntities.Single(t => t.ApplicationId == _applicationInfo.Id
                                                                                                   && t.EmailSent == true));
        }
    }
}
