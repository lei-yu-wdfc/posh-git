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
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);

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
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);

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

            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);

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

            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);
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
        [Row(3), Row(10), Row(11), Row(15), Row(19), Row(30)]
        public void VerifyUpdatedVariableInterestRateAppliedToLn(int loanTerm)
        {
            const decimal loanAmount = 100;
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);
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
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            l0Application.RepayOnDueDate();
            Do.With().Timeout(1).Until(() => l0Application.IsClosed);

            var lNApplication = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            lNApplication.RepayOnDueDate();

            Assert.IsTrue(Do.With().Timeout(1).Until(() => lNApplication.IsClosed));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1472")]
        public void VerifyMaxPercentageRateChargedOn30Days()
        {
            const int loanTerm = 30;
            const decimal maxRateOver30Days = (decimal)0.2100;
            const decimal loanAmount = 100;

            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);

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
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);
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
            ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);
            ConfigurationFunctions.SetVariableInterestRateEnabled(false);

            var customer = CustomerBuilder.New().ForProvince(Province).Build();
            var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
            ConfigurationFunctions.SetVariableInterestRateEnabled(true);

            l0Application.RepayOnDueDate();
            Do.With().Timeout(1).Until(() => l0Application.IsClosed);

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(l0Application.Id);

            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(12, 4), Row(15, 11)]
        //public void VerifyVariableInterestAmountAppliedToPartialEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;
        //    const int earlyRepaymentAmount = 50;
        //    ConfigurationFunctions.SetDelayBeforeApplicationClosed(0);

        //    var customer = CustomerBuilder.New().ForProvince(Province).Build();
        //    var l0Application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
        //    var applicationId = l0Application.Id;
        //    var accountId = l0Application.AccountId;

        //    l0Application.RepayEarly(earlyRepaymentAmount, earlyRepaymentTerm);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendPaymentTaken(applicationId);

        //    var expectedInterestAmountApplied =
        //        CalculateFunctionsCa.CalculateExpectedEarlyRepaymentVariableInterestAmountAppliedCa(loanAmount, loanTerm, earlyRepaymentTerm, earlyRepaymentAmount);
        //    var actualInterestAmoutApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
        //    Assert.IsTrue(actualInterestAmoutApplied == expectedInterestAmountApplied);
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(11, 5)] //, Row(25, 11)]
        //public void VerifyLoanClosedWithPartialEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;
        //    const int earlyRepaymentAmount = 50;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyL0LoanClosedWithPartialEarlyRepayment @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("earlyRepaymentTerm -> {0}", earlyRepaymentTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);
        //    TestLog.DebugTrace.WriteLine("earlyRepaymentAmount -> {0}", earlyRepaymentAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var earlyRepaymentDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(earlyRepaymentTerm);
        //    var adjustedEarlyRepaymentTerm = Convert.ToInt32(earlyRepaymentDate.Subtract(DateTime.Today).TotalDays.ToString());
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    var accountId = application["AccountId"];
        //    TestLog.DebugTrace.WriteLine("applicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, earlyRepaymentAmount.ToString(), accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendPaymentTaken(applicationId);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationClosed(applicationId));
        //    TestLog.DebugTrace.WriteLine("END: VerifyL0LoanClosedWithPartialEarlyRepayment @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(11, 5)]
        //public void VerifyLoanClosedWithFullEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyL0LoanClosedWithFullEarlyRepayment @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("earlyRepaymentTerm -> {0}", earlyRepaymentTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var earlyRepaymentDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(earlyRepaymentTerm);
        //    var adjustedEarlyRepaymentTerm = Convert.ToInt32(earlyRepaymentDate.Subtract(DateTime.Today).TotalDays.ToString());
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    var accountId = application["AccountId"];
        //    TestLog.DebugTrace.WriteLine("applicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    var earlyRepaymentAmount = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
        //                                                                                          earlyRepaymentTerm);
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, (earlyRepaymentAmount + loanAmount).ToString(), accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationClosed(applicationId));
        //    TestLog.DebugTrace.WriteLine("END: VerifyL0LoanClosedWithFullEarlyRepayment @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(11, 5)]
        //public void VerifyVariableInterestAmountPostForFullEarlyRepayment(int loanTerm, int earlyRepaymentTerm)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyVariableInterestAmountPostForFullEarlyRepayment @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("earlyRepaymentTerm -> {0}", earlyRepaymentTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var earlyRepaymentDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(earlyRepaymentTerm);
        //    var adjustedEarlyRepaymentTerm = Convert.ToInt32(earlyRepaymentDate.Subtract(DateTime.Today).TotalDays.ToString());
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    var accountId = application["AccountId"];
        //    TestLog.DebugTrace.WriteLine("applicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    var earlyRepaymentAmount = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
        //                                                                                          earlyRepaymentTerm);
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, (earlyRepaymentAmount + loanAmount).ToString(), accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationClosed(applicationId), waiting, interval);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount,
        //                                                                                                   earlyRepaymentTerm);
        //    var actualInterestAmoutApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
        //    TestLog.DebugTrace.WriteLine(
        //            "VerifyVariableInterestAmountCharged: actualAmount -> expectedAmount   {0} -> {1}", actualInterestAmoutApplied, expectedInterestAmountApplied);
        //    Assert.IsTrue(actualInterestAmoutApplied == expectedInterestAmountApplied);

        //    TestLog.DebugTrace.WriteLine("END: VerifyVariableInterestAmountPostForFullEarlyRepayment @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(15)]
        //public void VerifyLoanGoesIntoArrears(int loanTerm)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyL0LoanGoesIntoArrears @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId));

        //    TestLog.DebugTrace.WriteLine("END: VerifyL0LoanGoesIntoArrears @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(13)]
        //public void VerifyVariableInterestAmountPostedOnLoanInArrears(int loanTerm)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyVariableInterestAmountPostedOnLoanInArrears @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);

        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    var actualInterestAmoutApplied = commonFunct.GetInterestAmountApplied(applicationId);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmoutApplied, expectedInterestAmountApplied));
        //    TestLog.DebugTrace.WriteLine("END: VerifyVariableInterestAmountPostedOnLoanInArrears @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(25)]
        //public void VerifyArrearsInterestPostedToLoanInArrears(int loanTerm)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyArrearsInterestPosted @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);

        //    var actualRates = GetPaymentFunctions.GetInterestRatesForApplication(applicationId);
        //    var expectedArrearsRate = GetPaymentFunctions.GetCurrentArrearsInterestRate();

        //    TestLog.DebugTrace.WriteLine("actualArrearsRate -> expectedArrearsRate: {0} -> {1}", actualRates[actualRates.Count - 1].Mir, expectedArrearsRate);
        //    Assert.IsTrue(actualRates[actualRates.Count - 1].Mir == expectedArrearsRate);

        //    TestLog.DebugTrace.WriteLine("END: VerifyArrearsInterestPosted @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(17)]
        //public void VerifyDefaultChargeAppliedToLoanInArrears(int loanTerm)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyDefaultChargedAppliedToLoanInArrears @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    const decimal expectedDefaultChargeAmount = 20;
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);

        //    var actualDefaultChargeAmount = GetPaymentFunctions.GetActualDefaultChargeAmount(applicationId);
        //    TestLog.DebugTrace.WriteLine("actualDefaultChargeAmount -> expectedDefaultChargeAmount: {0} -> {1}", actualDefaultChargeAmount, expectedDefaultChargeAmount);
        //    Assert.IsTrue(actualDefaultChargeAmount == expectedDefaultChargeAmount);

        //    TestLog.DebugTrace.WriteLine("END: VerifyDefaultChargedAppliedToLoanInArrears @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(17, 3)]
        //public void VerifyLoanInArrearsClosedAfterAllPaymentsRecieved(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyLoanInArrearsClosedAfterAllPaymentsRecieved @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);
        //    TestLog.DebugTrace.WriteLine("daysOverdue -> {0}", daysOverdue);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);
        //    commonFunct.RewindQuery(applicationId, daysOverdue * -1);

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationClosed(applicationId));
        //    TestLog.DebugTrace.WriteLine("END: VerifyLoanInArrearsClosedAfterAllPaymentsRecieved @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(25, 9)]
        //public void VerifyArrearsInterestAmount(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyArrearsInterestAmount @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);
        //    TestLog.DebugTrace.WriteLine("daysOverdue -> {0}", daysOverdue);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);
        //    commonFunct.RewindQuery(applicationId, daysOverdue * -1);

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    commonFunct.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationClosed(applicationId), waiting, interval);

        //    var actualArrearsInterestAmountApplied = GetPaymentFunctions.GetArrearsInterestAmountApplied(applicationId);
        //    var expectedArrearsInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa((loanAmount + expectedInterestAmountApplied), daysOverdue);
        //    TestLog.DebugTrace.WriteLine("actualArrearsInterestAmountApplied -> expectedArrearsInterestAmountApplied: {0} -> {1}", actualArrearsInterestAmountApplied, expectedArrearsInterestAmountApplied);
        //    Assert.IsTrue(actualArrearsInterestAmountApplied == expectedArrearsInterestAmountApplied);

        //    TestLog.DebugTrace.WriteLine("END: VerifyArrearsInterestAmount @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(15, 6)]
        //public void VerifyDefaultChargePlusArrearsInterestCollected(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyDefaultChargePlusArrearsInterestCollected @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);
        //    TestLog.DebugTrace.WriteLine("daysOverdue -> {0}", daysOverdue);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    const decimal expectedDefaultChargeAmount = 20;
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);
        //    commonFunct.RewindQuery(applicationId, daysOverdue * -1);

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationClosed(applicationId), waiting, interval);

        //    var expectedArrearsInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedArrearsInterestAmountAppliedCa((loanAmount + expectedInterestAmountApplied), daysOverdue);
        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(applicationId, ((expectedDefaultChargeAmount + expectedArrearsInterestAmountApplied) * -1)));

        //    TestLog.DebugTrace.WriteLine("END: VerifyDefaultChargePlusArrearsInterestCollected @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(18, 12, 6)]
        //public void VerifyInterestChargedOnLoanInArrearsWithEarlyPaymentRecievedBeforeLoanDueDate(int loanTerm, int earlyRepaymentTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;
        //    const int earlyRepaymentAmount = 50;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyInterestChargedOnLoanInArrearsWithEarlyPaymentRecievedBeforeLoanDueDate @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("earlyRepaymentTerm -> {0}", earlyRepaymentTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);
        //    TestLog.DebugTrace.WriteLine("earlyRepaymentAmount -> {0}", earlyRepaymentAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var earlyRepaymentDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(earlyRepaymentTerm);
        //    var adjustedEarlyRepaymentTerm = Convert.ToInt32(earlyRepaymentDate.Subtract(DateTime.Today).TotalDays.ToString());
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() } });
        //    var applicationId = application["ApplicationId"];
        //    var accountId = application["AccountId"];
        //    TestLog.DebugTrace.WriteLine("applicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId, (adjustedEarlyRepaymentTerm * -1));
        //    SendPaymentFunctions.SendRepayLoanInternalViaBank(applicationId, earlyRepaymentAmount, accountId);
        //    SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), waiting, interval);
        //    commonFunct.RewindQuery(applicationId, daysOverdue * -1);

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);

        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedEarlyRepaymentVariableInterestAmountAppliedCa(loanAmount, loanTerm, earlyRepaymentTerm, earlyRepaymentAmount);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyDirectBankPaymentOfAmount(applicationId, (((loanAmount - earlyRepaymentAmount) + expectedInterestAmountApplied) * -1)));

        //    TestLog.DebugTrace.WriteLine("END: VerifyInterestChargedOnLoanInArrearsWithEarlyPaymentRecievedBeforeLoanDueDate @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(12, 16, 4)]
        //public void VerifyInterestChargedOnLoanInArrearsWithEarlyPaymentRecievedAfterLoanDueDate(int loanTerm, int earlyRepaymentTerm, int daysOverdue)
        //{

        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(18, 4)]
        //public void VerifyDefaultChargeNotAppliedToBcCustomer(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyDefaultChargeNotAppliedToBcCustomer @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() }, { "Province", "BC" } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyNoDefaultChargeApplied(applicationId));
        //    TestLog.DebugTrace.WriteLine("VerifyNoDefaultChargeApplied => true");
        //    TestLog.DebugTrace.WriteLine("END: VerifyDefaultChargeNotAppliedToBcCustomer @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-1472")]
        //[Row(8, 2)]
        //public void VerifyArrearsNotChargedToBcCustomer(int loanTerm, int daysOverdue)
        //{
        //    const decimal loanAmount = 100;

        //    TestLog.DebugTrace.WriteLine("BEGIN: VerifyArrearsNotChargedToBcCustomer @ TimeStamp: {0}\n", DateTime.UtcNow.ToString("o"));
        //    TestLog.DebugTrace.WriteLine("loanTerm -> {0}", loanTerm);
        //    TestLog.DebugTrace.WriteLine("loanAmount -> {0}", loanAmount);

        //    var promiseDate = (GetPaymentFunctions.GetNextWorkingDate(DateTime.Today)).AddDays(loanTerm).ToDateString();
        //    var waiting = new TimeSpan(0, 0, 1, 0);
        //    var interval = new TimeSpan(0, 0, 0, 10);
        //    SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

        //    var application =
        //        commonFunct.CreateL0Application(new Dictionary<string, string> { { "PromiseDate", promiseDate }, { "Amount", loanAmount.ToString() }, { "Province", "BC" } });
        //    var applicationId = application["ApplicationId"];
        //    TestLog.DebugTrace.WriteLine("ApplicationId: {0}\n", applicationId);

        //    commonFunct.RewindQuery(applicationId);
        //    TimeoutPaymentFunctions.TimeoutFixedTermLoanAndSchedInterest(applicationId);
        //    SendPaymentFunctions.SendTakePaymentFailed(applicationId);

        //    Do.Until(() => VerifyPaymentFunctions.VerifyApplicationInArrears(applicationId), interval, waiting);

        //    TimeoutPaymentFunctions.TimeoutPaymentsInArrearsSagaEntity(applicationId);
        //    Do.Sleep(2);
        //    SendPaymentFunctions.SendPaymentTakenForPaymentsInArrears(applicationId);
        //    var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, loanTerm);
        //    commonFunct.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

        //    Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationClosed(applicationId));

        //    TestLog.DebugTrace.WriteLine("END: VerifyArrearsNotChargedToBcCustomer @ TimeStamp: {0}", DateTime.UtcNow.ToString("o"));
        //}
    }
}
