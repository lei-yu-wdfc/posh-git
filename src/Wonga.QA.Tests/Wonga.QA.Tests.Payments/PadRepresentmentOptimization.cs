using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    public class PadRepresentmentOptimization
    {
        private readonly dynamic _opsSagasPaymentsInArrears = Drive.Data.OpsSagas.Db.PaymentsInArrearsSagaEntity;
        private readonly dynamic _opsSagasMultipleRepresentmentsForPaymentsInArrearsSagaEntity = 
                                                    Drive.Data.OpsSagas.Db.MultipleRepresentmentsForPaymentsInArrearsSagaEntity;

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey)]
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

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey)]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey)]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsIsSuccessfulThenASecondAttemptToRetrieve50PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey)]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey)]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsIsSuccessfulThenAThirdAttemptToRetrieveTheRemainingBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey)]
        public void WhenTheThirdPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey, true), ExpectedException(typeof(DoException))]
        public void WhenPadRepresentmentOptimizationFeatureSwitchIsOffThenTheMultipleRepresentmentsForPaymentsInArrearsSagaEntitySagaShouldNotBeUsed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            Do.Until(() => _opsSagasMultipleRepresentmentsForPaymentsInArrearsSagaEntity.FindByApplicationId(application.Id));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.PadRepresentmentOptimizationFeatureSwitchKey, true)]
        public void WhenPadRepresentmentOptimizationFeatureSwitchIsOffThenThePaymentsInArrearsSagaEntitySagaShouldBeUsed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            Do.Until(() => _opsSagasPaymentsInArrears.FindByApplicationId(application.Id));
        }


        private void TimeoutMultipleRepresentmentsForPaymentsInArrearsSagaEntity(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsForPaymentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = multipleRepresentmentSaga.Id });
        }

        private String GetRepresentmentsSent(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsForPaymentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.RepresentmentsSent;
        }

        private String GetNextPayDate(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsForPaymentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.NextPayDate;
        }
    }
}
