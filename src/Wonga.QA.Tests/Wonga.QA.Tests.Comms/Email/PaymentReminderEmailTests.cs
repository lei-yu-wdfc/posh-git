using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
    public abstract class PaymentReminderEmailTests
    {
        private string _mzTimeZone;
        private ProvinceEnum _province;

        private const string ConfigKey = "Comms.LocalTimeOfDayToSendPaymentReminderEmail";
        private const int MinuteDelayForEmail = 0;

        private string _oldConfigValue;
        private Customer _customer;
        private Application _application;
        private decimal _loanAmount;
        private int _loanTerm;

        [SetUp]
        public virtual void SetUp()
        {
            AdjustLocalTimeOfDayToSendPaymentReminderEmail();

            CreateApplication();
        }

        private void AdjustLocalTimeOfDayToSendPaymentReminderEmail()
        {
            ServiceConfigurationEntity configurationEntity =
                Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == ConfigKey);
            _oldConfigValue = configurationEntity.Value;

            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_mzTimeZone);
            DateTime currentLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone); 
            DateTime timeToSendEmail = currentLocalTime.AddMinutes(MinuteDelayForEmail);

            configurationEntity.Value = timeToSendEmail.TimeOfDay.ToString();

            configurationEntity.Submit();
        }

        private void CreateApplication()
        {
            _customer = CustomerBuilder.New().ForProvince(_province).Build();

            Console.WriteLine("Created Customer. Id: {0}", _customer.Id);

            _application = ApplicationBuilder.New(_customer).WithLoanAmount(_loanAmount).WithLoanTerm(_loanTerm).Build();

            Console.WriteLine("Created Application. ApplicationId: {0}", _application.Id);
        }

        [TearDown]
        public void TearDown()
        {
            ResetLocalTimeOfDayToSendPaymentReminderEmail();
        }

        private void ResetLocalTimeOfDayToSendPaymentReminderEmail()
        {
            ServiceConfigurationEntity configurationEntity =
                Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == ConfigKey);
            configurationEntity.Value = _oldConfigValue;

            configurationEntity.Submit();
        }

        private bool CheckPaymentReminderEmailSent()
        {
            return Driver.ThirdParties.ExactTarget.CheckPaymentReminderEmailSent(_customer.Email);
        }

        public abstract class GivenACustomerFromOntarioWithAnActiveLoan : PaymentReminderEmailTests
        {
            [SetUp]
            public override void SetUp()
            {
                _mzTimeZone = "Eastern Standard Time";
                _province = ProvinceEnum.ON;
                _loanAmount = 100;
                _loanTerm = 10;

                 base.SetUp();
            }

            public class WhenTheLoanIsDue : GivenACustomerFromOntarioWithAnActiveLoan
            {
                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    _application.MakeDueToday();
                }

                [Ignore("Ignored due to timing issues with exact target")]
                [Test, AUT(AUT.Ca), JIRA("CA-1149")]
                public void ThenAPaymentReminderEmailIsSent()
                {
                    Assert.IsTrue(CheckPaymentReminderEmailSent());
                }
            }

            public class WhenTheLoanHasAnEarlyPartialRepaymentAndIsDue : GivenACustomerFromOntarioWithAnActiveLoan
            {
                private decimal _earlyRepayment;
                private int _earlyRepaymentDay;

                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    _earlyRepayment = _loanAmount/2;
                    _earlyRepaymentDay = _loanTerm/2;

                    _application.RepayEarly(_earlyRepayment, _earlyRepaymentDay);

                    _application.MakeDueToday();
                }

                [Ignore("Ignored due to timing issues with exact target")]
                [Test, AUT(AUT.Ca), JIRA("CA-1149")]
                public void ThenAPaymentReminderEmailIsSent()
                {
                    Assert.IsTrue(CheckPaymentReminderEmailSent());
                }
            }
        }

        public abstract class GivenACustomerFromOntarioWithAnActiveVariableInterestLoan : PaymentReminderEmailTests
        {
            [SetUp]
            public override void SetUp()
            {
                _mzTimeZone = "Eastern Standard Time";
                _province = ProvinceEnum.ON;
                _loanAmount = 300;
                _loanTerm = 30;

                base.SetUp();
            }

            public class WhenTheLoanIsDue : GivenACustomerFromOntarioWithAnActiveVariableInterestLoan
            {
                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    _application.MakeDueToday();
                }

                [Ignore("Ignored due to timing issues with exact target")]
                [Test, AUT(AUT.Ca), JIRA("CA-1149")]
                public void ThenAPaymentReminderEmailIsSent()
                {
                    Assert.IsTrue(CheckPaymentReminderEmailSent());
                }
            }

            public class WhenTheLoanHasAnEarlyPartialRepaymentAndIsDue : GivenACustomerFromOntarioWithAnActiveVariableInterestLoan
            {
                private decimal _earlyRepayment;
                private int _earlyRepaymentDay;

                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    _earlyRepayment = _loanAmount / 2;
                    _earlyRepaymentDay = _loanTerm / 2;

                    _application.RepayEarly(_earlyRepayment, _earlyRepaymentDay);

                    _application.MakeDueToday();
                }

                [Ignore("Ignored due to timing issues with exact target")]
                [Test, AUT(AUT.Ca), JIRA("CA-1149")]
                public void ThenAPaymentReminderEmailIsSent()
                {
                    Assert.IsTrue(CheckPaymentReminderEmailSent());
                }
            }

        }

        public abstract class GivenACustomerFromOntarioWithAnActiveVariableInterestLoanScenario3 : PaymentReminderEmailTests
        {
            [SetUp]
            public override void SetUp()
            {
                _mzTimeZone = "Eastern Standard Time";
                _province = ProvinceEnum.ON;
                _loanAmount = 100;
                _loanTerm = 15;

                base.SetUp();
            }

            public class WhenTheLoanHasAnEarlyPartialRepaymentAndIsDue : GivenACustomerFromOntarioWithAnActiveVariableInterestLoanScenario3
            {
                private decimal _earlyRepayment;
                private int _earlyRepaymentDay;

                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    _earlyRepayment = 50;
                    _earlyRepaymentDay = 12;

                    _application.RepayEarly(_earlyRepayment, _earlyRepaymentDay);

                    _application.MakeDueToday();
                }

                [Ignore("Ignored due to timing issues with exact target")]
                [Test, AUT(AUT.Ca), JIRA("CA-1149")]
                public void ThenAPaymentReminderEmailIsSent()
                {
                    Assert.IsTrue(CheckPaymentReminderEmailSent());
                }
            }

            public class WhenTheLoanHasAnEarlyFullRepayment : GivenACustomerFromOntarioWithAnActiveVariableInterestLoanScenario3
            {
                private decimal _earlyRepayment;
                private int _earlyRepaymentDay;

                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    _earlyRepayment = 110.84m;
                    _earlyRepaymentDay = 12;

                    _application.RepayEarly(_earlyRepayment, _earlyRepaymentDay);

                    // Todo: Wait until closed
                }

                [Ignore("Ignored due to timing issues with exact target")]
                [Test, AUT(AUT.Ca), JIRA("CA-1149")]
                public void ThenAPaymentReminderEmailIsNotSent()
                {
                    Assert.IsFalse(CheckPaymentReminderEmailSent());
                }
            }
        }
    }

    [Obsolete("Cannot find any usages?.")]
    public class InterestRateCalculator
    {
        public int LoanTerm { get; set; }
        public decimal LoanAmount { get; set; }
        public int? EarlyRepaymentDay { get; set; }
        public decimal EarlyRepaymentAmount { get; set; }

        private decimal _defaultInterestRate = 30.416700m;
        private int _defaultInterestRateTerm = 10;

        private List<decimal> _variableInterestRates = new List<decimal>
                                                   {
                                                       25.397945m,
                                                       24.485444m,
                                                       23.572943m,
                                                       22.660442m,
                                                       21.747941m,
                                                       20.835440m,
                                                       19.922939m,
                                                       19.010438m,
                                                       18.097937m,
                                                       17.185436m,
                                                       16.272935m,
                                                       15.360434m,
                                                       14.447933m,
                                                       13.535432m,
                                                       12.622931m,
                                                       11.710430m,
                                                       10.797929m,
                                                       9.885428m,
                                                       8.972927m,
                                                       8.060426m
                                                   };

        public decimal GetBalance(int onDay = 0)
        {
            decimal accruedInterest = 0;
            decimal principle = LoanAmount;

            if (onDay == 0)
            {
                onDay = LoanTerm;
            }

            for (int i = 1; i <= onDay; i++)
            {
                decimal interestRate = i <= _defaultInterestRateTerm
                                           ? _defaultInterestRate
                                           : _variableInterestRates[i - _defaultInterestRateTerm - 1];

                if (EarlyRepaymentDay.HasValue && i == EarlyRepaymentDay.Value)
                {
                    principle = LoanAmount - EarlyRepaymentAmount;
                }

                decimal dailyInterestRate = interestRate*12/365/100;

                accruedInterest += principle*dailyInterestRate;
            }

            return principle + accruedInterest;
        }

        public void PaymentSenario3()
        {
            LoanTerm = 15;
            LoanAmount = 100;
            EarlyRepaymentDay = 12;
            EarlyRepaymentAmount = 50;

            const decimal expectedBallance = 62.36m;

            decimal actualBalance = GetBalance();

            Assert.AreEqual(expectedBallance, Math.Round(actualBalance, 2));

        }

    }

}