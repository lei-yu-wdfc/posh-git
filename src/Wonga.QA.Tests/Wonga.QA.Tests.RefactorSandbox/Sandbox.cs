using MbUnit.Framework;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Application;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.RefactorSandbox
{
	[TestFixture]
	public class Sandbox
	{
		private ConsumerAccount _consumerAccount;
		private ConsumerApplication _consumerApplication;

		[Test, AUT(AUT.Uk), Explicit]
		public void AccountBuilderConsumerTest()
		{
			_consumerAccount = AccountBuilder.Consumer.New().Build();
		}

		[Test, AUT(AUT.Uk), Explicit]
		public void ApplicationBuilderConsumerTest()
		{
			_consumerApplication = ApplicationBuilder.Consumer.New(_consumerAccount).Build();
		}
	}
}
