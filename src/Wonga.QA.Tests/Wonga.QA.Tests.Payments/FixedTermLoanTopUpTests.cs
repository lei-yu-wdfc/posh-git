using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, JIRA("UK-426", "UK-789"), AUT(AUT.Uk), Parallelizable(TestScope.All)]
    [TestsOn(typeof(GetAccountSummaryQuery)), TestsOn(typeof(GetAccountOptionsUkQuery)), TestsOn(typeof(GetFixedTermLoanTopupOfferQuery)), TestsOn(typeof(GetFixedTermLoanTopupCalculationQuery)), TestsOn(typeof(CreateFixedTermLoanTopupCommand)), TestsOn(typeof(GetLoanTopUpAgreementQuery)), TestsOn(typeof(SignFixedTermLoanTopupCommand))]
    public class FixedTermLoanTopUpTests
    {
        private Int32 _limit;
        private Int32 _loan;
        private Int32 _topup;
        private Decimal _fee;

        [FixtureSetUp]
        public void Defaults()
        {
            _limit = Int32.Parse(Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Risk.DefaultCreditLimit").Value);
            _loan = _limit / 2;
            _topup = (_limit - _loan) / 2;
            _fee = Drive.Data.Payments.Db.Products.FindByName("WongaFixedLoan").TransmissionFee;
        }

        #region GetAccountSummary

        [Test]
        public void GetAccountSummaryHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();
            var summary = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });

            Assert.IsTrue(Boolean.Parse(summary.Values["CurrentLoanTopUpAvailable"].Single()));
            Assert.AreEqual(_limit - _loan, Decimal.Parse(summary.Values["AvailableCredit"].Single()));
        }

        [Test]
        public void GetAccountSummaryNegativeTest()
        {
            var customer = CustomerBuilder.New().Build();
            var summary = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });

            Assert.IsFalse(Boolean.Parse(summary.Values["CurrentLoanTopUpAvailable"].Single()));
            Assert.AreEqual(_limit, Decimal.Parse(summary.Values["AvailableCredit"].Single()));

            var application = ApplicationBuilder.New(customer).WithLoanAmount(_limit).Build();
            summary = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });

            Assert.IsFalse(Boolean.Parse(summary.Values["CurrentLoanTopUpAvailable"].Single()));
            Assert.AreEqual(0, Decimal.Parse(summary.Values["AvailableCredit"].Single()));
        }

        [Test, Explicit, Factory("Boundaries")]
        public void GetAccountSummaryBoundaryTest(Int32 loan)
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loan).Build();
            var summary = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });

            Assert.IsTrue(Boolean.Parse(summary.Values["CurrentLoanTopUpAvailable"].Single()));
            Assert.AreEqual(_limit - loan, Decimal.Parse(summary.Values["AvailableCredit"].Single()));
        }

        #endregion

        #region GetAccountOptions

        [Test]
        public void GetAccountOptionsHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();
            var options = GetOptions(Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = customer.Id, TrustRating = _limit }).Root);

            Assert.ContainsKey(options, "RequestCredit");
            Assert.IsTrue(options["RequestCredit"]);
        }

        [Pending("UK-1872")]
        [Test]
        public void GetAccountOptionsNegativeTest()
        {
            var customer = CustomerBuilder.New().Build();
            var options = GetOptions(Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = customer.Id, TrustRating = _limit }).Root);

            Assert.DoesNotContainKey(options, "RequestCredit");

            var application = ApplicationBuilder.New(customer).WithLoanAmount(_limit).Build();
            options = GetOptions(Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = customer.Id, TrustRating = _limit }).Root);

            Assert.DoesNotContainKey(options, "RequestCredit");
        }

        [Pending("UK-1697")]
        [Test, Explicit, Factory("Boundaries")]
        public void GetAccountOptionsBoundaryTest(Int32 loan)
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loan).Build();
            var options = GetOptions(Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = customer.Id, TrustRating = _limit }).Root);

            Assert.ContainsKey(options, "RequestCredit");
            Assert.IsTrue(options["RequestCredit"]);
        }

        private Dictionary<String, Boolean> GetOptions(XElement options)
        {
            var ns = options.GetDefaultNamespace();
            return options.Descendants(ns.GetName("ProductOption")).ToDictionary(e => e.Element(ns.GetName("OptionKey")).Value, e => Boolean.Parse(e.Element(ns.GetName("Enabled")).Value));
        }

        #endregion

        #region GetFixedTermLoanTopupOffer

        [Pending("UK-1873")]
        [Test]
        public void GetFixedTermLoanTopupOfferHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();
            var offer = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });

            Assert.AreEqual(application.Id, Guid.Parse(offer.Values["ApplicationId"].Single()));
            Assert.IsTrue(Boolean.Parse(offer.Values["IsEnabled"].Single()));
            Assert.AreEqual(_limit - _loan, Decimal.Parse(offer.Values["AmountMax"].Single()));
            Assert.AreEqual(_loan + _fee, Decimal.Parse(offer.Values["TotalToRepay"].Single()));
            Assert.AreEqual(_fee, Decimal.Parse(offer.Values["TransmissionFee"].Single()));

            foreach (var key in new[] { "InterestRateMonthly", "InterestRateAnnual", "InterestRateAPR", "DaysTillRepaymentDate" })
                Assert.GreaterThan(Decimal.Parse(offer.Values[key].Single()), 0, key);
        }

        [Test, Explicit]
        public void CreateFixedTermLoanTopupMultipleTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var id = Guid.NewGuid();
            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = id,
                TopupAmount = _topup - 1
            });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(id));

            Drive.Api.Commands.Post(new SignFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                FixedTermLoanTopupId = id
            });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(id).SignedOn);

            id = Guid.NewGuid();
            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = id,
                TopupAmount = 1
            }));
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(id));
        }

        [Pending("UK-1874")]
        [Test, MultipleAsserts]
        public void GetFixedTermLoanTopupOfferNegativeTest()
        {
            var customer = CustomerBuilder.New().Build();
            var offer = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });

            Assert.IsNull(offer.Values["ApplicationId"].Single());
            Assert.IsFalse(Boolean.Parse(offer.Values["IsEnabled"].Single()));
            Assert.AreEqual(0, Decimal.Parse(offer.Values["AmountMax"].Single()));

            var application = ApplicationBuilder.New(customer).WithLoanAmount(_limit).Build();
            offer = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });

            Assert.IsNotNull(offer.Values["ApplicationId"].Single());
            Assert.AreEqual(application.Id, Guid.Parse(offer.Values["ApplicationId"].Single()));
            Assert.IsFalse(Boolean.Parse(offer.Values["IsEnabled"].Single()));
            Assert.AreEqual(0, Decimal.Parse(offer.Values["AmountMax"].Single()));
        }

        [Pending("UK-1875")]
        [Test, Explicit, MultipleAsserts]
        public void GetFixedTermLoanTopupOfferNegativeTests()
        {
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery()));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = Guid.NewGuid() }));
        }

        [Test, Explicit, Factory("Boundaries")]
        public void GetFixedTermLoanTopupOfferBoundaryTest(Int32 loan)
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loan).Build();
            var offer = Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = customer.Id });

            Assert.AreEqual(application.Id, Guid.Parse(offer.Values["ApplicationId"].Single()));
            Assert.IsTrue(Boolean.Parse(offer.Values["IsEnabled"].Single()));
            Assert.AreEqual(_limit - loan, Decimal.Parse(offer.Values["AmountMax"].Single()));
        }

        #endregion

        #region GetFixedTermLoanTopupCalculation

        [Test]
        public void GetFixedTermLoanTopupCalculationHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();
            var calculation = Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = application.Id, TopupAmount = _topup });

            var fee = Decimal.Parse(calculation.Values["InterestAndFeesAmount"].Single());
            Assert.GreaterThan(fee, _fee);
            Assert.AreEqual(fee + _topup, Decimal.Parse(calculation.Values["TotalRepayable"].Single()));
        }

        [Pending("UK-1876")]
        [Test, MultipleAsserts]
        public void GetFixedTermLoanTopupCalculationNegativeTest()
        {
            var customer = CustomerBuilder.New().Build();
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = Guid.NewGuid(), TopupAmount = _topup }));

            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();
            foreach (var topup in new[] { -1, 0, _limit - _loan + 1 })
                Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = application.Id, TopupAmount = topup }), "Topup: {0}", topup);
        }

        [Test, Explicit, Factory("Boundaries")]
        public void GetFixedTermLoanTopupCalculationBoundaryTest(Int32 loan)
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loan).Build();
            var calculation = Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = application.Id, TopupAmount = _limit - loan });

            var fee = Decimal.Parse(calculation.Values["InterestAndFeesAmount"].Single());
            Assert.GreaterThan(fee, _fee);
            Assert.AreEqual(fee + _limit - loan, Decimal.Parse(calculation.Values["TotalRepayable"].Single()));
        }

        [Test, Explicit, MultipleAsserts]
        public void GetFixedTermLoanTopupCalculationNegativeTests()
        {
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery()));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = Guid.NewGuid() }));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { TopupAmount = _topup }));
        }

        #endregion

        #region CreateFixedTermLoanTopup

        [Test]
        public void CreateFixedTermLoanTopupHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var id = Guid.NewGuid();
            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = id,
                TopupAmount = _topup
            });
            var topup = Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(id));

            Assert.AreEqual(application.Id, Drive.Data.Payments.Db.Applications.Get(topup.ApplicationId).ExternalId);
            Assert.AreEqual(topup.Amount, _topup);
            Assert.IsNull(topup.SignedOn);
        }

        [Test, Description("Payments_Topup_LoanAmount_IsMax")]
        public void CreateFixedTermLoanTopupMaxTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_limit).Build();

            var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = 1
            }));
            Assert.Contains(exception.Errors, "Payments_Topup_LoanAmount_IsMax");
        }

        [Pending("UK-1871")]
        [Test]
        public void CreateFixedTermLoanTopupInArrearsTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();
            application.PutApplicationIntoArrears();

            Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = _topup
            }));
        }

        [Test, Explicit, Description("Payments_Topup_InProgress")]
        public void CreateFixedTermLoanTopupInProgessTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var topup = Guid.NewGuid();
            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = topup,
                TopupAmount = _topup
            });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindAllByExternalId(topup));

            var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = _topup
            }));
            Assert.Contains(exception.Errors, "Payments_Topup_InProgress");
        }

        [Pending("UK-1877")]
        [Test, Explicit, Description("Payments_TopUp_TopupAmount_Invalid")]
        public void CreateFixedTermLoanTopupAmountInvalidTests()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            foreach (var topup in new[] { -1, 0, _limit - _loan + 1 })
            {
                var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
                {
                    AccountId = customer.Id,
                    ApplicationId = application.Id,
                    FixedTermLoanTopupId = Guid.NewGuid(),
                    TopupAmount = topup
                }), "Topup: {0}", topup);
                Assert.Contains(new[] { "Payments_TopUp_TopupAmount_Invalid", "Payments_Topup_AmountGreaterThanAvailableCredit" }, exception.Errors.Single());
            }
        }

        [Test, Explicit, Factory("Boundaries")]
        public void CreateFixedTermLoanTopupBoundaryTests(Int32 loan)
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(loan).Build();

            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = _limit - loan
            }));
        }

        [Test, Explicit]
        public void CreateFixedTermLoanTopupRaceTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
            Drive.Api.Commands.Post(new[]
	        {
		        new CreateFixedTermLoanTopupCommand
		        {
			        AccountId = customer.Id,
			        ApplicationId = application.Id,
			        FixedTermLoanTopupId = ids[0],
			        TopupAmount = _topup
		        },
		        new CreateFixedTermLoanTopupCommand
		        {
			        AccountId = customer.Id,
			        ApplicationId = application.Id,
			        FixedTermLoanTopupId = ids[1],
			        TopupAmount = _topup
		        }
	        });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindAllByExternalId(ids));
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Assert.AreEqual(1, Drive.Data.Payments.Db.Topups.FindAllByExternalId(ids).Count());
        }

        [Test, Explicit, Description("Payments_Topup_ApplicationId_NotEqual_LastOpenedApplication")]
        public void CreateFixedTermLoanTopupWrongApplicationTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = Guid.NewGuid(),
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = _topup
            }));
            Assert.Contains(exception.Errors, "Payments_Topup_ApplicationId_NotEqual_LastOpenedApplication");
        }

        [Test, Explicit, Description("Payments_Topup_NoOpenedApplication")]
        public void CreateFixedTermLoanTopupNoApplicationTests()
        {
            var customer = CustomerBuilder.New().WithMiddleName("TESTEmployedMask").WithEmployerStatus("Unemployed").Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = Guid.NewGuid(),
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = _topup
            }));
            Assert.Contains(exception.Errors, "AccountId_NotFound");

            exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = _topup
            }));
            Assert.Contains(exception.Errors, "Payments_Topup_NoOpenedApplication");
        }

        [Test, Explicit, MultipleAsserts]
        public void CreateFixedTermLoanTopupNegativeTests()
        {
            Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand()));
            Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand { FixedTermLoanTopupId = Guid.NewGuid(), TopupAmount = _topup }));
        }

        #endregion

        #region GetLoanTopUpAgreement

        [Test]
        public void GetLoanTopUpAgreementHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var id = Guid.NewGuid();
            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = id,
                TopupAmount = _topup
            });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(id));

            var agreement = Do.Until(() => Drive.Api.Queries.Post(new GetLoanTopUpAgreementQuery { AccountId = customer.Id, FixedTermLoanTopupId = id }));
            Assert.Contains(agreement.Values["AgreementContent"].Single(), String.Format("&pound;{0}", _topup));
        }

        [Test, Explicit, Description("Comms_LoanAgreement_NotPresent")]
        public void GetLoanTopUpAgreementNegativeTest()
        {
            var customer = CustomerBuilder.New().Build();
            var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetLoanTopUpAgreementQuery { AccountId = customer.Id, FixedTermLoanTopupId = Guid.NewGuid() }));
            Assert.Contains(exception.Errors, "Comms_LoanAgreement_NotPresent");
        }

        #endregion

        #region SignFixedTermLoanTopup

        [Test]
        public void SignFixedTermLoanTopupHappyPathTest()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(_loan).Build();

            var id = Guid.NewGuid();
            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = id,
                TopupAmount = _topup
            });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(id));

            Drive.Api.Commands.Post(new SignFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                FixedTermLoanTopupId = id
            });
            Do.Until<Boolean>(() => Drive.Data.Payments.Db.Transactions.FindAllByComponentTransactionId(id).Count() == 2);

            var signed = Drive.Data.Payments.Db.Topups.FindByExternalId(id).SignedOn;
            Assert.AreEqual(DateTime.Today, signed.Date);

            var response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });
            Assert.AreEqual(_loan + _topup, Decimal.Parse(response.Values["LoanAmount"].Single()));
            Assert.AreEqual(_fee * 2, Decimal.Parse(response.Values["Fees"].Single()));
        }

        [Test, Explicit, Description("Payments_TopUp_FixedTermLoanTopupId_Invalid")]
        public void SignFixedTermLoanTopupNegativeTest()
        {
            var customer = CustomerBuilder.New().Build();
            var exception = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new SignFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                FixedTermLoanTopupId = Guid.NewGuid()
            }));
            Assert.Contains(exception.Errors, "Payments_TopUp_FixedTermLoanTopupId_Invalid");
        }

        #endregion

        private Int32[] Boundaries()
        {
            return new[] { 1, _limit - 1 };
        }

        [Pending("UK-1878")]
        [Test, Explicit, ExpectedException(typeof(ValidatorException))]
        public void MillionPoundTransaction()
        {
            var customer = CustomerBuilder.New().WithMiddleName("Foo").WithEmployer("Bar").Build();
            var application = Guid.NewGuid();
            var topup = Guid.NewGuid();

            Drive.Api.Commands.Post(CreateFixedTermLoanApplicationCommand.New(c =>
            {
                c.AccountId = customer.Id;
                c.ApplicationId = application;
                c.BankAccountId = customer.GetBankAccount();
                c.PaymentCardId = customer.GetPaymentCard();
            }));
            Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(application));

            Drive.Api.Commands.Post(new SignApplicationCommand
            {
                AccountId = customer.Id,
                ApplicationId = application
            });
            Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(application).SignedOn);

            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application,
                FixedTermLoanTopupId = topup,
                //TopupAmount = 1000000
                TopupAmount = 1
            });
            Do.Until(() => Drive.Data.Payments.Db.Topups.FindByExternalId(topup));

            Drive.Api.Commands.Post(new SignFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                FixedTermLoanTopupId = topup
            });
            Do.Until<Boolean>(() => Drive.Data.Payments.Db.Transactions.FindAllByComponentTransactionId(topup).Any());
        }
    }
}
