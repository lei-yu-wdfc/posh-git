using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using IncomeFrequencyEnum = Wonga.QA.Framework.Api.IncomeFrequencyEnum;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class PadRepresentmentOptimization
    {
        private readonly dynamic _opsSagasPaymentsInArrears = Drive.Data.OpsSagas.Db.PaymentsInArrearsSagaEntity;
        private readonly dynamic _opsSagasMultipleRepresentmentsInArrearsSagaEntity =
                                                    Drive.Data.OpsSagas.Db.MultipleRepresentmentsInArrearsSagaEntity;
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;
        private readonly dynamic _inArrearsNoticeSaga = Drive.Data.OpsSagas.Db.InArrearsNoticeSagaEntity;

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenACustomerGoesIntoArrearsThenAnAttemptToRetrieve33PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {
            const double percentageToBeCollectedForRepresentmentOne = 0.33;
            const int loanTerm = 10;
            const decimal principle = 100;
            const decimal interest = 10;
            const decimal defaultCharge = 20;
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            var arrearsInterestForRepresentmentOne =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principle + interest), numOfDaysToNextPayDateForRepresentmentOne));

            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "0");
            Assert.IsTrue(GetNextRepresentmentDate(application.Id) == nextPayDateForRepresentmentOne.ToString());

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            var transactionForRepresentmentOne = _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               OrderByTransactionIdDescending().First();

            var amountToBeCollectedForRepresentmentOne =
                (decimal)(((double)(arrearsInterestForRepresentmentOne + principle + interest + defaultCharge)) * percentageToBeCollectedForRepresentmentOne);

            var amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentOne, 2, MidpointRounding.AwayFromZero);

            Assert.IsTrue(transactionForRepresentmentOne.Amount == amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces);
            Assert.IsTrue(CurrentRepresentmentAmount(application.Id) == amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces);
            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "1");
            Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(application.Id, -amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {
            const double percentageToBeCollectedForRepresentmentOne = 0.33;
            const int loanTerm = 10;
            const decimal principle = 100;
            const decimal interest = 10;
            const decimal defaultCharge = 20;
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            var arrearsInterestForRepresentmentOne =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principle + interest), numOfDaysToNextPayDateForRepresentmentOne));

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Failed) == 2);

            var transactionForRepresentmentOne = _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).
                               OrderByTransactionIdDescending().First();

            var amountToBeCollectedForRepresentmentOne =
                (decimal)(((double)(arrearsInterestForRepresentmentOne + principle + interest + defaultCharge)) * percentageToBeCollectedForRepresentmentOne);

            var amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentOne, 2, MidpointRounding.AwayFromZero);

            Assert.IsTrue(transactionForRepresentmentOne.Amount == amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces);

            //TODO: add assert to ensure the saga is no longer exists in the db...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsIsSuccessfulThenASecondAttemptToRetrieve50PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {
            const double percentageToBeCollectedForRepresentmentOne = 0.33;
            const double percentageToBeCollectedForRepresentmentTwo = 0.50;
            const int loanTerm = 10;
            const decimal principle = 100;
            const decimal interest = 10;
            const decimal defaultCharge = 20;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.BiWeekly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            var arrearsInterestForRepresentmentOne =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principle + interest), numOfDaysToNextPayDateForRepresentmentOne));

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            var amountToBeCollectedForRepresentmentOne =
                (decimal)(((double)(arrearsInterestForRepresentmentOne + principle + interest + defaultCharge)) * percentageToBeCollectedForRepresentmentOne);

            var amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentOne, 2, MidpointRounding.AwayFromZero);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.PutApplicationFurtherIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Paid) == 3);

            var transactionForRepresentmentTwo = _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                   OrderByTransactionIdDescending().First();

            var principleBalanceAfterRepresentmentOne = principle - amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces;

            var arrearsInterestForRepresentmentTwo =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principleBalanceAfterRepresentmentOne + interest), numOfDaysToNextPayDateForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwo =
                (decimal)(((double)((arrearsInterestForRepresentmentOne + arrearsInterestForRepresentmentTwo + principleBalanceAfterRepresentmentOne + interest + defaultCharge))
                                                                                                                                    * percentageToBeCollectedForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentTwo, 2, MidpointRounding.AwayFromZero);

            Assert.IsTrue(transactionForRepresentmentTwo.Amount == amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces);
            Assert.IsTrue(CurrentRepresentmentAmount(application.Id) == amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces);
            Assert.IsTrue(GetNumberOfRepresentmentsSent(application.Id) == "2");
            Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(application.Id, -amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces));

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {
            const double percentageToBeCollectedForRepresentmentOne = 0.33;
            const double percentageToBeCollectedForRepresentmentTwo = 0.50;
            const int loanTerm = 10;
            const decimal principle = 100;
            const decimal interest = 10;
            const decimal defaultCharge = 20;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.BiWeekly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            var arrearsInterestForRepresentmentOne =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principle + interest), numOfDaysToNextPayDateForRepresentmentOne));

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            var amountToBeCollectedForRepresentmentOne =
                (decimal)(((double)(arrearsInterestForRepresentmentOne + principle + interest + defaultCharge)) * percentageToBeCollectedForRepresentmentOne);

            var amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentOne, 2, MidpointRounding.AwayFromZero);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.PutApplicationFurtherIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentTwo);

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Failed) == 2);

            var transactionForRepresentmentTwo = _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).
                   OrderByTransactionIdDescending().First();

            var principleBalanceAfterRepresentmentOne = principle - amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces;

            var arrearsInterestForRepresentmentTwo =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principleBalanceAfterRepresentmentOne + interest), numOfDaysToNextPayDateForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwo =
                (decimal)(((double)((arrearsInterestForRepresentmentOne + arrearsInterestForRepresentmentTwo + principleBalanceAfterRepresentmentOne + interest + defaultCharge))
                                                                                                                                    * percentageToBeCollectedForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentTwo, 2, MidpointRounding.AwayFromZero);

            Assert.IsTrue(transactionForRepresentmentTwo.Amount == amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces);

            //TODO: add assert to ensure the saga is no longer exists in the db...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsIsSuccessfulThenAThirdAttemptToRetrieveTheRemainingBalanceShouldBeMadeOnTheNextPayDate()
        {
            const double percentageToBeCollectedForRepresentmentOne = 0.33;
            const double percentageToBeCollectedForRepresentmentTwo = 0.50;
            const int loanTerm = 10;
            const decimal principle = 100;
            const decimal interest = 10;
            const decimal defaultCharge = 20;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.BiWeekly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            var arrearsInterestForRepresentmentOne =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principle + interest), numOfDaysToNextPayDateForRepresentmentOne));

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            var amountToBeCollectedForRepresentmentOne =
                (decimal)(((double)(arrearsInterestForRepresentmentOne + principle + interest + defaultCharge)) * percentageToBeCollectedForRepresentmentOne);

            var amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentOne, 2, MidpointRounding.AwayFromZero);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.PutApplicationFurtherIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Paid) == 3);

            var principleBalanceAfterRepresentmentOne = principle - amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces;

            var arrearsInterestForRepresentmentTwo =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principleBalanceAfterRepresentmentOne+interest), numOfDaysToNextPayDateForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwo =
                (decimal)(((double)((arrearsInterestForRepresentmentOne + arrearsInterestForRepresentmentTwo + principleBalanceAfterRepresentmentOne + interest + defaultCharge)) 
                                                                                                                                    * percentageToBeCollectedForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentTwo, 2, MidpointRounding.AwayFromZero);

            var currentDateForRepresentmentThree = Convert.ToDateTime(nextPayDateForRepresentmentTwo);

            var nextPayDateForRepresentmentThree = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentThree, Convert.ToDateTime(nextPayDateForRepresentmentTwo),
                                                                (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentThree = (int)nextPayDateForRepresentmentThree.Subtract(currentDateForRepresentmentThree).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentThree);
            application.PutApplicationFurtherIntoArrears((uint) numOfDaysToNextPayDateForRepresentmentThree);
            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                           _bgTrans.TransactionStatus ==
                           (int)BankGatewayTransactionStatus.Paid) == 4);

            var transactionForRepresentmentThree = _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).OrderByTransactionIdDescending().First();

            var principleBalanceAfterRepresentmentTwo = principleBalanceAfterRepresentmentOne - amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces;

            var arrearsInterestForRepresentmentThree =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principleBalanceAfterRepresentmentTwo + interest), numOfDaysToNextPayDateForRepresentmentThree));

            var amountToBeCollectedForRepresentmentThree =
                (decimal)(((double)((arrearsInterestForRepresentmentOne + arrearsInterestForRepresentmentTwo + arrearsInterestForRepresentmentThree 
                                        + principleBalanceAfterRepresentmentTwo + interest + defaultCharge))));

            var amountToBeCollectedForRepresentmentThreeRoundedToTwoDecimalPlaces =
                        Decimal.Round(amountToBeCollectedForRepresentmentThree, 2, MidpointRounding.AwayFromZero);

            Assert.IsTrue(transactionForRepresentmentThree.Amount == amountToBeCollectedForRepresentmentThreeRoundedToTwoDecimalPlaces);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(application.Id, -amountToBeCollectedForRepresentmentThreeRoundedToTwoDecimalPlaces));
            Assert.IsTrue(Do.With.Timeout(1).Until(() => application.IsClosed));
            //TODO: add assert to ensure the saga is no longer exists in the db...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), FeatureSwitch(FeatureSwitchConstants.MultipleRepresentmentsInArrearsFeatureSwitchKey)]
        public void WhenTheThirdPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {
            const double percentageToBeCollectedForRepresentmentOne = 0.33;
            const double percentageToBeCollectedForRepresentmentTwo = 0.50;
            const int loanTerm = 10;
            const decimal principle = 100;
            const decimal interest = 10;
            const decimal defaultCharge = 20;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.BiWeekly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutApplicationIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            var arrearsInterestForRepresentmentOne =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principle + interest), numOfDaysToNextPayDateForRepresentmentOne));

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            var amountToBeCollectedForRepresentmentOne =
                (decimal)(((double)(arrearsInterestForRepresentmentOne + principle + interest + defaultCharge)) * percentageToBeCollectedForRepresentmentOne);

            var amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentOne, 2, MidpointRounding.AwayFromZero);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.PutApplicationFurtherIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Paid) == 3);

            var principleBalanceAfterRepresentmentOne = principle - amountToBeCollectedForRepresentmentOneRoundedToTwoDecimalPlaces;

            var arrearsInterestForRepresentmentTwo =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principleBalanceAfterRepresentmentOne + interest), numOfDaysToNextPayDateForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwo =
                (decimal)(((double)((arrearsInterestForRepresentmentOne + arrearsInterestForRepresentmentTwo + principleBalanceAfterRepresentmentOne + interest + defaultCharge))
                                                                                                                                    * percentageToBeCollectedForRepresentmentTwo));

            var amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces =
                Decimal.Round(amountToBeCollectedForRepresentmentTwo, 2, MidpointRounding.AwayFromZero);

            var currentDateForRepresentmentThree = Convert.ToDateTime(nextPayDateForRepresentmentTwo);

            var nextPayDateForRepresentmentThree = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentThree, Convert.ToDateTime(nextPayDateForRepresentmentTwo),
                                                                (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentThree = (int)nextPayDateForRepresentmentThree.Subtract(currentDateForRepresentmentThree).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentThree);
            application.PutApplicationFurtherIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentThree);

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                           _bgTrans.TransactionStatus ==
                           (int)BankGatewayTransactionStatus.Failed) == 2);

            var transactionForRepresentmentThree = _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).OrderByTransactionIdDescending().First();

            var principleBalanceAfterRepresentmentTwo = principleBalanceAfterRepresentmentOne - amountToBeCollectedForRepresentmentTwoRoundedToTwoDecimalPlaces;

            var arrearsInterestForRepresentmentThree =
                (CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa(
                    (principleBalanceAfterRepresentmentTwo + interest), numOfDaysToNextPayDateForRepresentmentThree));

            var amountToBeCollectedForRepresentmentThree =
                (decimal)(((double)((arrearsInterestForRepresentmentOne + arrearsInterestForRepresentmentTwo + arrearsInterestForRepresentmentThree
                                        + principleBalanceAfterRepresentmentTwo + interest + defaultCharge))));

            var amountToBeCollectedForRepresentmentThreeRoundedToTwoDecimalPlaces =
                        Decimal.Round(amountToBeCollectedForRepresentmentThree, 2, MidpointRounding.AwayFromZero);

            Assert.IsTrue(transactionForRepresentmentThree.Amount == amountToBeCollectedForRepresentmentThreeRoundedToTwoDecimalPlaces);

            //TODO: add assert to ensure the saga is no longer exists in the db...
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
            return multipleRepresentmentSaga.RepresentmentsSent.ToString();
        }

        private String GetNextRepresentmentDate(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.NextRepresentmentDate.ToString();
        }

        private Decimal CurrentRepresentmentAmount(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return Math.Round(Convert.ToDecimal(multipleRepresentmentSaga.LastRepresentmentAmount), 2, MidpointRounding.AwayFromZero);
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
