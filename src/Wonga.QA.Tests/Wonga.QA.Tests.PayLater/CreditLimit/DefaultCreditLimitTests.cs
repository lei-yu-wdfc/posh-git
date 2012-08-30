using MbUnit.Framework;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using ApplicationBuilder = Wonga.QA.Framework.Builders.ApplicationBuilder;

namespace Wonga.QA.Tests.PayLater.CreditLimit
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class DefaultCreditLimitTests
	{
        private const decimal ExpectedPayLaterCreditLimit = 1000;


        [Test]
        private void PayLaterCreditLimit_ShouldBeSetToDefaultValue_WhenCreatingPayLaterApplication()
        {
        	var account = AccountBuilder.PayLater.New().Build();
            ApplicationBuilder.PayLater.New(account).Build();
			
            Assert.AreEqual(ExpectedPayLaterCreditLimit, account.PayLaterCredit);
        }

        [Test]
        private void PayLaterCreditLimit_ShouldBeSetToDefaultValue_WhenCreatingConsumerLoanApplication()
        {
            var account = AccountBuilder.Consumer.New().Build();
            ApplicationBuilder.Consumer.New(account).Build();
            
            Assert.AreEqual(ExpectedPayLaterCreditLimit, account.PayLaterCredit);
        }
    }
}
    