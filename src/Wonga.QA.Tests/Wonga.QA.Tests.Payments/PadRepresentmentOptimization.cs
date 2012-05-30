using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;
using Wonga.QA.Tests.Payments.Helpers;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Payments
{
    public class PadRepresentmentOptimization
    {
        private readonly dynamic _opsSagasPaymentsInArrears = Drive.Data.OpsSagas.Db.PaymentsInArrearsSagaEntity;
        private readonly dynamic _opsSagasMultipleRepresentmentsInArrearsSagaEntity =
                                                    Drive.Data.OpsSagas.Db.MultipleRepresentmentsInArrearsSagaEntity;
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;
        private readonly dynamic _inArrearsNoticeSaga = Drive.Data.OpsSagas.Db.InArrearsNoticeSagaEntity;

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenACustomerGoesIntoArrearsThenAnAttemptToRetrieve33PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {
            const double totalAmountOwed = 130.00; //TODO: update this to include arrears interest - will do this when tests are up and running...
            const int percentageToBeCollected = 33;
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();

            var nextPayDate = Convert.ToDateTime(customer.GetNextPayDate());
            var numOfDaysToNextPayDate = (int)nextPayDate.Subtract(DateTime.Today).TotalDays; //TODO: Might need to change the substracted date to promise date or something...

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDate);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDate);

            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "0");
            Assert.IsTrue(GetNextRepresentmentDate(application.Id) == DateHelper.GetNextWorkingDayForCa(nextPayDate).ToString());

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => _bgTrans.GetCount(_bgTrans.ApplicationId == application.Id) == 3);

            var transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               OrderByDescending(_bgTrans.TransactionId).First());

            Assert.IsTrue(transaction.Amout == (totalAmountOwed / percentageToBeCollected));
            Assert.IsTrue(Convert.ToDouble(GetCurrentAmount(application.Id)) == (totalAmountOwed / percentageToBeCollected));
            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "1");
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {
            const double totalAmountOwed = 130.00; //TODO: update this to include arrears interest - will do this when tests are up and running...
            const int percentageToBeCollected = 33;
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();

            var nextPayDate = Convert.ToDateTime(customer.GetNextPayDate());
            var numOfDaysToNextPayDate = (int)nextPayDate.Subtract(DateTime.Today).TotalDays; //TODO: Might need to change the substracted date to promise date or something...

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDate);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDate);

            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "0");
            Assert.IsTrue(GetNextRepresentmentDate(application.Id) == DateHelper.GetNextWorkingDayForCa(nextPayDate).ToString());

            ScotiaResponseBuilder.New().
                    ForBankAccountNumber(customer.BankAccountNumber).
                    Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => _bgTrans.GetCount(_bgTrans.ApplicationId == application.Id) == 3);

            var transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).
                               OrderByDescending(_bgTrans.TransactionId).First());

            Assert.IsTrue(transaction.Amout == (totalAmountOwed / percentageToBeCollected));

            //TODO: add assert to ensure the saga is no longer exists in the db...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsIsSuccessfulThenASecondAttemptToRetrieve50PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {
            const double totalAmountOwed = 130.00; //TODO: update this to include arrears interest - will do this when tests are up and running...
            const int percentageToBeCollected = 50;
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();

            var nextPayDate = Convert.ToDateTime(customer.GetNextPayDate());
            var numOfDaysToNextPayDate = (int)nextPayDate.Subtract(DateTime.Today).TotalDays; //TODO: Might need to change the substracted date to promise date or something...

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDate);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDate);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => _bgTrans.GetCount(_bgTrans.ApplicationId == application.Id) == 3);

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               OrderByDescending(_bgTrans.TransactionId).First());

            var nextPayDateTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today.AddDays(numOfDaysToNextPayDate + 1), 
                                                                                nextPayDate, (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));

            var numOfDaysBetweenThePayDates = (int)nextPayDateTwo.Subtract(nextPayDate).TotalDays;
            application.PutApplicationIntoArrears((uint)numOfDaysBetweenThePayDates);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysBetweenThePayDates);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => _bgTrans.GetCount(_bgTrans.ApplicationId == application.Id) == 4);

            var transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               OrderByDescending(_bgTrans.TransactionId).First());

            Assert.IsTrue(transaction.Amout == (totalAmountOwed / percentageToBeCollected));
            Assert.IsTrue(Convert.ToDouble(GetCurrentAmount(application.Id)) == (totalAmountOwed / percentageToBeCollected));
            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "2");
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsIsSuccessfulThenAThirdAttemptToRetrieveTheRemainingBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore(), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheThirdPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey, true), ExpectedException(typeof(DoException))]
        public void WhenPadRepresentmentOptimizationFeatureSwitchIsOffThenTheMultipleRepresentmentsForPaymentsInArrearsSagaEntitySagaShouldNotBeUsed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(application.Id));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey, true)]
        public void WhenPadRepresentmentOptimizationFeatureSwitchIsOffThenThePaymentsInArrearsSagaEntitySagaShouldBeUsed()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears();

            Do.Until(() => _opsSagasPaymentsInArrears.FindByApplicationId(application.Id));
        }


        private void TimeoutMultipleRepresentmentsInArrearsSagaEntity(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = multipleRepresentmentSaga.Id });
        }

        private String GetNumberOfRepresentmentsSent(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.RepresentmentsSent;
        }

        private String GetNextRepresentmentDate(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.NextRepresentmentDate;
        }

        private String GetCurrentAmount(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.CurrentAmount;
        }

        private void TimeoutInArrearsNoticeSaga(Guid applicationGuid, int numberOfDaysInArrears)
        {
            var inArrearsNoticeSaga =
                Do.Until(() => _inArrearsNoticeSaga.FindByApplicationId(applicationGuid));

            for (var i = 0; i < numberOfDaysInArrears; i++)
            {
                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = inArrearsNoticeSaga.Id });
            }
        }
    }
}
