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
    [Parallelizable(TestScope.Self)]
    class PaymentFailureEmails
    {
        private BusinessApplication _applicationInfo;

        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();
            _applicationInfo = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build() as BusinessApplication;
        }

        [Test, JIRA("SME-810")]
        public void EmailIsSentToCustomerOnCollectionFailure()
        {
            Assert.IsNotNull(_applicationInfo.GetPaymentPlan());
            _applicationInfo.MorningCollectionAttempt(null, false, false);
            _applicationInfo.AfternoonCollectionAttempt(false);

            var firstPayReqFailedSagaTab = Drive.Data.OpsSagas.Db.FirstPaymentRequestFailedSagaEntity;
            Do.Until(
                () =>
                firstPayReqFailedSagaTab.FindAll(firstPayReqFailedSagaTab.ApplicationId == _applicationInfo.Id &&
                                                 firstPayReqFailedSagaTab.EmailSent == true).Single());

            var secondPayReqFailedSagaTab = Drive.Data.OpsSagas.Db.SecondPaymentRequestFailedSagaEntity;
            Do.Until(
                () => secondPayReqFailedSagaTab.FindAll(secondPayReqFailedSagaTab.ApplicationId == _applicationInfo.Id
                                                        && secondPayReqFailedSagaTab.EmailSent == true).Single());
        }
    }
}
