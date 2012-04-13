using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Payments
{
    // Todo: These tests assume that the variable interest rates start on day 11 and end on day 30
    // Todo: This assumption will cause the tests to break whenever the variable interest rate config in the DB changes
    // Todo: Therefore these tests should be driven by the DB config and should not make any assumptions    
    public class VariablePricingTests
    {
        private const ProvinceEnum Province = ProvinceEnum.ON;

        private List<VariableInterestRateDetailEntity> _originalRates;
        private int _closeDelay;

        [SetUp]
        public void Setup()
        {
            _originalRates = GetPaymentFunctions.GetCurrentVariableInterestRates();
            _closeDelay = ConfigurationFunctions.GetDelayBeforeApplicationClosed();
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);
        }

        [TearDown]
        public void TearDown()
        {
            SetPaymentFunctions.SetCurrentVariableInterestRates(_originalRates);
            ConfigurationFunctions.SetVariableInterestRateEnabled(true);
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(_closeDelay);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        public void VerifyFixedTermLoanOfferCaQueryRates()
        {
            var response = GetPaymentFunctions.GetFixedTermLoanOfferCaQuery();
            var actualVariableRates = GetPaymentFunctions.GetVariableRatesFromApiResponse(response);
            var expectedVariableRates = GetPaymentFunctions.GetCurrentVariableInterestRates();

            Assert.IsTrue(VerifyPaymentFunctions.VerifyFixedTermLoanOfferQueryRates(actualVariableRates, expectedVariableRates));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(11), Row(10), Row(11), Row(15), Row(19), Row(30)]
        public void VerifyVariableInterestPostedOnLoanCreation(int loanTerm)
        {
            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();
            var applicationId = application.Id;

            var actualRates = GetPaymentFunctions.GetInterestRatesForApplication(applicationId);
            var expectedRates = GetPaymentFunctions.GetCurrentVariableInterestRates(loanTerm);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestRatesApplied(actualRates, expectedRates));
        }


        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(12), Row(10), Row(11), Row(15), Row(19), Row(30)]
        public void VerifyInterestAmountCharged(int loanTerm)
        {
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            application.RepayOnDueDate();

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);

            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(9), Row(15)]
        public void VerifyLoanClosedAfterPaymentFullRecieved(int loanTerm)
        {
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            application.RepayOnDueDate();

            Assert.IsTrue(application.IsClosed);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        public void VerifyFixedTermLoanOfferQueryRates()
        {
            var response = GetPaymentFunctions.GetFixedTermLoanOfferCaQuery();
            var actualVariableRates = GetPaymentFunctions.GetVariableRatesFromApiResponse(response);
            var expectedVariableRates = GetPaymentFunctions.GetCurrentVariableInterestRates();

            Assert.IsTrue(VerifyPaymentFunctions.VerifyFixedTermLoanOfferQueryRates(actualVariableRates, expectedVariableRates));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        public void VerifyFixedTermLoanOfferQueryRatesAfterRatesUpdated()
        {
            const decimal increment = (decimal)0.3;
            SetPaymentFunctions.IncrementVariableInterestRatesMonthlyInterestRate(increment);

            var response = GetPaymentFunctions.GetFixedTermLoanOfferCaQuery();
            var actualVariableRates = GetPaymentFunctions.GetVariableRatesFromApiResponse(response);
            var expectedVariableRates = GetPaymentFunctions.GetCurrentVariableInterestRates();

            Assert.IsTrue(VerifyPaymentFunctions.VerifyFixedTermLoanOfferQueryRates(actualVariableRates, expectedVariableRates));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(1), Row(10), Row(11), Row(15), Row(19), Row(30)]
        public void VerifyCurrentVariableInterestAppliedAfterRatesUpdated(int loanTerm)
        {
            const decimal loanAmount = 100;
            const decimal increment = (decimal)0.3;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
                                                                                                       loanTerm);
            SetPaymentFunctions.IncrementVariableInterestRatesMonthlyInterestRate(increment);

            application.RepayOnDueDate();

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(application.Id);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(11), Row(15), Row(19), Row(30)]
        public void VerifyUpdatedVariableInterestRateAppliedToLn(int loanTerm)
        {
            const decimal loanAmount = 100;
            const decimal increment = (decimal)0.3;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            l0Application.RepayOnDueDate();

            SetPaymentFunctions.IncrementVariableInterestRatesMonthlyInterestRate(increment);

            var lNApplication = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            lNApplication.RepayOnDueDate();

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(lNApplication.Id);
            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);

            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(10), Row(15)]
        public void VerifyLNLoanClosedAfterPaymentRecieved(int loanTerm)
        {
            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            l0Application.RepayOnDueDate();
            Do.With.Timeout(1).Until(() => l0Application.IsClosed);

            var lNApplication = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            lNApplication.RepayOnDueDate();

            Assert.IsTrue(Do.With.Timeout(1).Until(() => lNApplication.IsClosed));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        public void VerifyMaxPercentageRateChargedOn30Days()
        {
            const int loanTerm = 30;
            const decimal maxRateOver30Days = (decimal)0.2100;
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            var actualRatesApplied = GetPaymentFunctions.GetInterestRatesForApplication(l0Application.Id);
            var actualDailyRateApplied = CalculateFunctionsCa.CalculateActualDailyInterestRateCa(actualRatesApplied);

            Assert.IsTrue(actualDailyRateApplied == maxRateOver30Days);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(17)]
        public void VerifyNoVariableInterestPostedWhenFeatureSwitchIsOff(int loanTerm)
        {
            const decimal loanAmount = 100;
            ConfigurationFunctions.SetVariableInterestRateEnabled(false);

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            var actualRates = GetPaymentFunctions.GetInterestRatesForApplication(l0Application.Id);
            var expectedRates = GetPaymentFunctions.GetCurrentVariableInterestRates(loanTerm);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestRatesApplied(actualRates, expectedRates));

            ConfigurationFunctions.SetVariableInterestRateEnabled(true);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(22)]
        public void VerifyExistingLoansInterestRatesAreNotAffectedIfFeatureSwitchIsTurnedOn(int loanTerm)
        {
            const decimal loanAmount = 100;
            ConfigurationFunctions.SetVariableInterestRateEnabled(false);

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
            ConfigurationFunctions.SetVariableInterestRateEnabled(true);

            l0Application.RepayOnDueDate();
            Do.With.Timeout(1).Until(() => l0Application.IsClosed);

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(l0Application.Id);

            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        //WIP FROM HERE DOWN....

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(12, 4), Row(15, 11)]
        //public void VerifyVariableInterestAmountAppliedToPartialEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;
        //    const int earlyRepaymentAmount = 50;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    l0Application.RepayEarly(earlyRepaymentAmount, earlyRepaymentTerm);
        //    l0Application.RepayOnDueDate();

        //    var expectedInterestAmountApplied =
        //        CalculateFunctionsCa.CalculateExpectedEarlyRepaymentVariableInterestAmountAppliedCa(loanAmount, loanTerm, earlyRepaymentTerm, earlyRepaymentAmount);
        //    var actualInterestAmoutApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
        //    Assert.IsTrue(actualInterestAmoutApplied == expectedInterestAmountApplied);
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(11, 5), Row(25, 11)]
        //public void VerifyLoanClosedWithPartialEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;
        //    const int earlyRepaymentAmount = 50;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;
        //    var accountId = l0Application.AccountId;

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, earlyRepaymentAmount.ToString(), accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendPaymentTaken(applicationId);
            
        //    Assert.IsTrue(Do.With().Timeout(1).Until(() => l0Application.IsClosed));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(11, 5)]
        //public void VerifyLoanClosedWithFullEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;
        //    var accountId = l0Application.AccountId;

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    var earlyRepaymentAmount = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
        //                                                                                          earlyRepaymentTerm);
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, (earlyRepaymentAmount + loanAmount).ToString(), accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    Assert.IsTrue(Do.With().Timeout(1).Until(() => l0Application.IsClosed));
        //}

       // [Test, AUT(AUT.Ca), JIRA("CA-1472")]
       // [Row(11, 5)]
       // public void VerifyVariableInterestAmountPostForFullEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
       // {
       //     const decimal loanAmount = 100;

       //     var customer = CustomerBuilder.New().ForProvince(Province).Build();
       //     var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
       //     var applicationId = l0Application.Id;
       //     var accountId = l0Application.AccountId;
 
       //     commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
       //     var earlyRepaymentAmount = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
       //                                                                                           earlyRepaymentTerm);
       //     SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, (earlyRepaymentAmount + loanAmount).ToString(), accountId);
       //     SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

       //     Do.With().Timeout(1).Until(() => l0Application.IsClosed);
       //     var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
       //                                                                                                    earlyRepaymentTerm);
       //     var actualInterestAmoutApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
       //     Assert.IsTrue(actualInterestAmoutApplied == expectedInterestAmountApplied);
       //}

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(15)]
        public void VerifyLoanGoesIntoArrears(int loanTerm)
        {
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = l0Application.Id;

            l0Application.PutApplicationIntoArrears();

            Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));
        }

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(13)]
        //public void VerifyVariableInterestAmountPostedOnLoanInArrears(int loanTerm)
        //{
        //    const decimal loanAmount = 100;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.With().Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    var actualInterestAmoutApplied = commonFunct.GetInterestAmountApplied(applicationId);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmoutApplied, expectedInterestAmountApplied));
        //}

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(25)]
        public void VerifyArrearsInterestPostedToLoanInArrears(int loanTerm)
        {
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = l0Application.Id;

            l0Application.PutApplicationIntoArrears();

            Do.With.Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

            var actualRates = GetPaymentFunctions.GetInterestRatesForApplication(applicationId);
            var expectedArrearsRate = GetPaymentFunctions.GetCurrentArrearsInterestRate();
            Assert.IsTrue(actualRates[actualRates.Count - 1].Mir == expectedArrearsRate);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(17)]
        public void VerifyDefaultChargeAppliedToLoanInArrears(int loanTerm)
        {
            const decimal loanAmount = 100;

            const decimal expectedDefaultChargeAmount = 20;

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = l0Application.Id;

            l0Application.PutApplicationIntoArrears();

            Do.With.Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

            var actualDefaultChargeAmount = GetPaymentFunctions.GetActualDefaultChargeAmount(applicationId);
            Assert.IsTrue(actualDefaultChargeAmount == expectedDefaultChargeAmount);
        }

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(17, 3)]
        //public void VerifyLoanInArrearsClosedAfterAllPaymentsRecieved(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    l0Application.PutApplicationIntoArrears(daysOverdue);

        //    Do.With().Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    Assert.IsTrue(Do.With().Timeout(1).Until(() => l0Application.IsClosed));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(25, 9)]
        //public void VerifyArrearsInterestAmount(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    l0Application.PutApplicationIntoArrears(daysOverdue);

        //    Do.With().Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    commonFunct.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);

        //    Do.With().Timeout(1).Until(() => l0Application.IsClosed);

        //    var actualArrearsInterestAmountApplied = GetPaymentFunctions.GetArrearsInterestAmountApplied(applicationId);
        //    var expectedArrearsInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa((loanAmount + expectedInterestAmountApplied), daysOverdue);
        //    Assert.IsTrue(actualArrearsInterestAmountApplied == expectedArrearsInterestAmountApplied);
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(15, 6)]
        //public void VerifyDefaultChargePlusArrearsInterestCollected(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    const decimal expectedDefaultChargeAmount = 20;

        //    var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.BC).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    l0Application.PutApplicationIntoArrears(daysOverdue);

        //    Do.With().Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);

        //    Do.With().Timeout(1).Until(() => l0Application.IsClosed);

        //    var expectedArrearsInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa((loanAmount + expectedInterestAmountApplied), daysOverdue);
        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(applicationId, ((expectedDefaultChargeAmount + expectedArrearsInterestAmountApplied) * -1)));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(18, 12, 6)]
        //public void VerifyInterestChargedOnLoanInArrearsWithEarlyPaymentRecievedBeforeLoanDueDate(int loanTerm, int earlyRepaymentTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;
        //    const int earlyRepaymentAmount = 50;

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, earlyRepaymentAmount, accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.With().Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));
        //    commonFunct.RewindQuery(applicationId, daysOverdue * -1);

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);

        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedEarlyRepaymentVariableInterestAmountAppliedCa(loanAmount, loanTerm, earlyRepaymentTerm, earlyRepaymentAmount);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(applicationId, (((loanAmount - earlyRepaymentAmount) + expectedInterestAmountApplied) * -1)));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(12, 16, 4)]
        //public void VerifyInterestChargedOnLoanInArrearsWithEarlyPaymentRecievedAfterLoanDueDate(int loanTerm, int earlyRepaymentTerm, int daysOverdue)
        //{

        //}

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        [Row(18, 4)]
        public void VerifyDefaultChargeNotAppliedToBcCustomer(int loanTerm, uint daysOverdue)
        {
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.BC).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = l0Application.Id;

            l0Application.PutApplicationIntoArrears(daysOverdue);

            Do.With.Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

            Assert.IsTrue(VerifyPaymentFunctions.VerifyNoDefaultChargeApplied(applicationId));
        }

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(8, 2)]
        //public void VerifyArrearsInterestNotChargedToBcCustomer(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.BC).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;

        //    l0Application.PutApplicationIntoArrears(daysOverdue);

        //    Do.With().Timeout(1).Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    commonFunct.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    Assert.IsTrue(Do.With().Timeout(1).Until(() => l0Application.IsClosed));
        //}
    }
}
