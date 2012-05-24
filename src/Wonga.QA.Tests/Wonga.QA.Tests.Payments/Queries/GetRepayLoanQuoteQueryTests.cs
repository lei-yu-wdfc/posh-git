using System;
using System.Diagnostics;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using CreateFixedTermLoanApplicationCommand = Wonga.QA.Framework.Api.CreateFixedTermLoanApplicationCommand;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.PaymentTransactionScopeEnum;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture, AUT(AUT.Uk), JIRA("UK-1827"), Parallelizable(TestScope.All), TestsOn(typeof(GetRepayLoanQuoteUkQuery))]
    public class GetRepayLoanQuoteQueryTests
    {
        private Decimal _amin;
        private Decimal _amax;
        private Int32 _tmin;
        private Int32 _tmax;
        private Decimal _fee;

        [FixtureSetUp]
        public void Defaults()
        {
            var product = Drive.Data.Payments.Db.Products.FindByName("WongaFixedLoan");
            var limit = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Risk.DefaultCreditLimit");

            _amin = product.AmountMin;
            _amax = Decimal.Parse(limit.Value);
            _tmin = (Int32)product.TermMin;
            _tmax = (Int32)product.TermMax;
            _fee = product.TransmissionFee + product.ServiceFee;

            Trace.WriteLine(String.Format("AmountMin: {0}, CreditLimit: {1}, TermMin: {2}, TermMax: {3}, Fee: {4}", _amin, _amax, _tmin, _tmax, _fee));
        }

        [Test, MultipleAsserts]
        public void GetRepayLoanQuoteHappyPath()
        {
            var amount = Get.RandomInt((Int32)_amin, (Int32)_amax);
            var term = Get.RandomInt(_tmin, _tmax);

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer)
                .WithLoanAmount(amount)
                .WithLoanTerm(term)
                .Build();

            var response = Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = application.Id });
            var quote = new Quote(response);

            Assert.AreEqual(application.Id, quote.Application);
            Assert.GreaterThanOrEqualTo(quote.Min, _amin);
            Assert.AreEqual(amount + _fee, quote.Max);
            Assert.AreEqual(term, quote.Days);
            Assert.Count((Int32)Math.Ceiling(amount + _fee - quote.Min), quote.Remainders);

            for (var i = 0; i < quote.Remainders.Length - 1; i++)
                Assert.GreaterThan(quote.Remainders[i], quote.Remainders[i + 1]);
        }

        [Test, Explicit, MultipleAsserts]
        public void GetRepayLoanQuoteNegative()
        {
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery()));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = Guid.NewGuid() }));

            var command = CreateFixedTermLoanApplicationCommand.New();
            Drive.Api.Commands.Post(command);
            Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(command.ApplicationId));

            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = command.ApplicationId }));
        }

        [Test, Explicit]
        public void GetRepayLoanQuoteRepaid()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            Assert.DoesNotThrow(() => Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = application.Id }));
            application.RepayOnDueDate();
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = application.Id }));
        }

        [Pending("UK-1827")]
        [Test, Explicit, Factory("Boundaries")]
        public void GetRepayLoanQuoteBoundary(Decimal amount)
        {
            var command = CreateFixedTermLoanApplicationCommand.New();
            Drive.Api.Commands.Post(command);
            Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(command.ApplicationId));

            var message = new CreateTransactionCommand
            {
                ExternalId = Guid.NewGuid(),
                ApplicationId = (Guid)command.ApplicationId,
                Amount = amount,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Scope = PaymentTransactionScopeEnum.Credit,
                Source = PaymentTransactionSourceEnum.Manual,
                Type = PaymentTransactionEnum.CashAdvance,
                PostedOn = DateTime.Now
            };
            Drive.Msmq.Payments.Send(message);
            Do.Until(() => Drive.Data.Payments.Db.Transactions.FindByExternalId(message.ExternalId));

            var quote = new Quote(Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery { ApplicationId = command.ApplicationId }));
            
            Assert.AreEqual(amount, quote.Min);
            Assert.AreEqual(amount, quote.Max);
            Assert.IsEmpty(quote.Remainders);
        }

        private Decimal[] Boundaries()
        {
            return new[] { 0.01m, _amin - 0.01m, _amin };
        }

        private class Quote
        {
            public Guid Application { get; set; }
            public Decimal Min { get; set; }
            public Decimal Max { get; set; }
            public Int32 Days { get; set; }
            public Decimal[] Remainders { get; set; }

            public Quote(ApiResponse response)
            {
                Application = Guid.Parse(response.Values["ApplicationId"].Single());
                Min = Decimal.Parse(response.Values["SliderMinAmount"].Single());
                Max = Decimal.Parse(response.Values["SliderMaxAmount"].Single());
                Days = Int32.Parse(response.Values["DaysToDueDate"].Single());
                Remainders = response.Values["Amount"].Select(Decimal.Parse).ToArray();
            }
        }

        [Test]
        public void RepayEarlyOnLoanStartDate()
        {
            const int dueInDays = 10;
            var promiseDate = new Date(DateTime.UtcNow.AddDays(dueInDays));
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new RepayLoanFunctions();
            setupData.RepayEarlyOnLoanStartDate(appId, paymentCardId, bankAccountId, accountId, trustRating, dueInDays);

            //Call Api Query
            var response = Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery() { ApplicationId = appId });
            var minRepayAmount = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.RepayLoanMinAmount").Value;

            Assert.AreEqual(appId.ToString(), response.Values["ApplicationId"].Single(), "ApplicationId incorrect");
            Assert.AreEqual(minRepayAmount, response.Values["SliderMinAmount"].Single(), "SliderMinAmount incorrect");
            Assert.AreEqual("105.50", response.Values["SliderMaxAmount"].Single(), "SliderMaxAmount incorrect");
            Assert.AreEqual("10", response.Values["DaysToDueDate"].Single(), "DaysToDueDate incorrect");

            // Check array
            Assert.AreEqual(110.41M, decimal.Parse(response.Values["Amount"].ToArray()[0]));
        }
    }
}
