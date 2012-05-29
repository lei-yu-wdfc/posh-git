using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class PadRepresentmentOptimization
    {
        private readonly dynamic _opsSagasPaymentsInArrears = Drive.Data.OpsSagas.Db.PaymentsInArrearsSagaEntity;

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenACustomerGoesIntoArrearsThenAnAttemptToRetrieve33PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            Assert.IsTrue(GetRepresentmentsSent(application.Id) == "1");
            Assert.IsTrue(GetNextPayDate(application.Id) == customer.GetNextPayDate());

            TimeoutMultipleRepresentmentsForPaymentsInArrearsSagaEntity(application.Id);

            Assert.IsTrue(GetRepresentmentsSent(application.Id) == "2");
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsIsSuccessfulThenASecondAttemptToRetrieve50PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsIsSuccessfulThenAThirdAttemptToRetrieveTheRemainingBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheThirdPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        private void TimeoutMultipleRepresentmentsForPaymentsInArrearsSagaEntity(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasPaymentsInArrears.FindByApplicationId(applicationGuid));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = multipleRepresentmentSaga.Id });
        }

        private String GetRepresentmentsSent(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasPaymentsInArrears.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.RepresentmentsSent;
        }

        private String GetNextPayDate(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasPaymentsInArrears.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.NextPayDate;
        }
    }
}
