using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Account.Consumer;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using ApplicationBuilder = Wonga.QA.Framework.Builders.ApplicationBuilder;

namespace Wonga.QA.Tests.PayLater.TrustRating
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class DefaultTrustRatingTests
    {
        private const string DefaultPayLaterCreditLimit = "Risk.DefaultPayLaterCreditLimit";
        private decimal? _expectedPayLaterCreditLimit;

        [SetUp]
        private void SetUp()
        {
            _expectedPayLaterCreditLimit = decimal.Parse(Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == DefaultPayLaterCreditLimit).Value);
        }

        [Test]
        [Ignore("Waiting on PAYLATER-879 for ApplicationBuilder to be working")]
        private void PayLaterCreditLimit_ShouldBeSetToDefaultValue_WhenCreatingPayLaterApplication()
        {
            PayLaterAccount acc = AccountBuilder.PayLater.New().Build();
            ApplicationBuilder.PayLater.New(acc).Build();

            Assert.AreEqual(_expectedPayLaterCreditLimit, GetPayLaterCreditLimit(acc));
        }

        [Test]
        [Ignore("Not returning correct result as PayLaterCreditLimit is currently created as null by risk")]
        private void PayLaterCreditLimit_ShouldBeSetToDefaultValue_WhenCreatingConsumerLoanApplication()
        {
            ConsumerAccount acc = AccountBuilder.Consumer.New().Build();
            ApplicationBuilder.Consumer.New(acc).Build();
            
            Assert.AreEqual(_expectedPayLaterCreditLimit, GetPayLaterCreditLimit(acc));
        }

        private decimal? GetPayLaterCreditLimit(AccountBase account)
        {
            //should be the following but PayLaterCreditLimit is currently null so won't work
            //return Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == account.Id).PayLaterCreditLimit);

            Do.Until(() => Drive.Db.Risk.RiskAccounts.Any(a => a.AccountId == account.Id));
            return Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == account.Id).PayLaterCreditLimit;
        }
    }
}
    