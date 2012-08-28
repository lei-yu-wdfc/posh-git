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
    public class DefaultCreditLimitTests
    {
        private const decimal ExpectedPayLaterCreditLimit = 1000m;

        [Test]
        [Ignore("PayLater ApplicationBuilder not fully working")]
        private void PayLaterCreditLimit_ShouldBeSetToDefaultValue_WhenCreatingPayLaterApplication()
        {
            PayLaterAccount acc = AccountBuilder.PayLater.New().Build();
            ApplicationBuilder.PayLater.New(acc).Build();

            Assert.AreEqual(ExpectedPayLaterCreditLimit, GetPayLaterCreditLimit(acc));
        }

        [Test]
        private void PayLaterCreditLimit_ShouldBeSetToDefaultValue_WhenCreatingConsumerLoanApplication()
        {
            ConsumerAccount acc = AccountBuilder.Consumer.New().Build();
            ApplicationBuilder.Consumer.New(acc).Build();

            Assert.AreEqual(ExpectedPayLaterCreditLimit, GetPayLaterCreditLimit(acc));
        }

        private decimal? GetPayLaterCreditLimit(AccountBase account)
        {
            return Do.Until(() => Drive.Db.Risk.RiskAccounts.Single(a => a.AccountId == account.Id).PayLaterCreditLimit);
        }
    }
}
    