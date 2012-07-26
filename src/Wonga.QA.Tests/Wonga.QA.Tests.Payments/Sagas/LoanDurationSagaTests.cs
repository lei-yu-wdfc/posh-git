using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Sagas
{
    [TestFixture]
    [AUT(AUT.Uk)]
    [JIRA("UK-2009")]
    public abstract class LoanDurationSagaTests
    {
        private bool _originalUseLoanDurationSagaValue;
        protected Application _application;
        protected Customer _customer;
        protected Guid _cardId;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _originalUseLoanDurationSagaValue = Drive.Data.Ops.GetServiceConfiguration<bool>("Payments.FeatureSwitches.UseLoanDurationSaga");
            Drive.Data.Ops.SetServiceConfiguration<bool>("Payments.FeatureSwitches.UseLoanDurationSaga", true);
        }

        [FixtureTearDown]
        public void FixtureTeardown()
        {
            Drive.Data.Ops.SetServiceConfiguration<bool>("Payments.FeatureSwitches.UseLoanDurationSaga", _originalUseLoanDurationSagaValue);
        }

        public virtual void Setup()
        {
            _customer = CustomerBuilder.New()
                                       .Build();

            Do.With.Timeout(2).Interval(10).Until(() => null != Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_customer.Id).SingleOrDefault() &&
                                                        null != Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_customer.Id).Single().PaymentCardsBase);

            _cardId = _customer.GetPaymentCard();

            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(100)
                                             .WithLoanTerm(7)
                                             .Build();
        }

        public class GivenACustomerWithAnApprovedLoan : LoanDurationSagaTests
        {
            protected dynamic _loanSignups = Drive.Data.Payments.Db.LoanSignups;
            protected int _maxLoanSignup;

            public override void Setup()
            {
                _maxLoanSignup = (_loanSignups.All().OrderByDescending(_loanSignups.Id).First()).Id;
                base.Setup();
            }

            public class ALoanSignupRecordIsAdded : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test]
                [AUT(AUT.Uk), Owner(Owner.SeamusHoban)]
                public void Confirmed()
                {
                    Do.With.Timeout(2).Interval(10).Until(() => null != _loanSignups.FindAll(_loanSignups.Id > _maxLoanSignup));
                }
            }

            public class WhenTheLoanIsDue : GivenACustomerWithAnApprovedLoan
            {
                protected dynamic _transactions = Drive.Data.Payments.Db.Transactions;
                protected dynamic _applications = Drive.Data.Payments.Db.Applications;
                protected decimal _balanceToday = 0m; 
                protected int _applicationId;

                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                    //Make the application due today. Can't use the QA method.. it only works with the old sagas...
                    _balanceToday = _application.GetBalanceToday();
                    _applicationId = _applications.FindBy(ExternalId: _application.Id).ApplicationId;
                    _application.MakeDueToday();
                }

                [Test]
                [AUT(AUT.Uk)]
                [JIRA("UK-2310/UK-2009"), Owner(Owner.SeamusHoban)]
                public void PaymentIsTakenForTheLoan()
                {
                    // Using <= because at the time of writing there is a bug in the accrued interest calculator

                    Do.With.Timeout(2).Interval(10).Until(() => _application.GetBalance() <= 0m);

                    Do.With.Timeout(2).Interval(10).Until(() => null != _transactions.FindAll(_transactions.ApplicationId == _applicationId && 
                                                                                              _transactions.Amount <= -_balanceToday && // <= because of bug with balance calculation...
                                                                                              _transactions.Scope == (byte)PaymentTransactionScopeEnum.Credit &&
                                                                                              _transactions.Type == "CardPayment").FirstOrDefault()); 
                }
            }
        }
    }
}
