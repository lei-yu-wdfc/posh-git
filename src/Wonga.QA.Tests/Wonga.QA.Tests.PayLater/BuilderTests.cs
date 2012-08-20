using MbUnit.Framework;
using Wonga.QA.Framework.Account.PayLater;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.PayLater
{
	[TestFixture, AUT(AUT.Uk), Category(TestCategories.SmokeTest)]
    public class BuilderTests
	{
		private PayLaterAccount _account;

		[Test]
		public void AccountBuilder_CanBuild_AnAccount()
		{
			_account = AccountBuilder.PayLater.New().Build();
		}

		[Test, DependsOn("ApplicationBuilder_CanBuild_AnAccount"), Pending("Until ApplicationBuilder is completed")]
		public void ApplicationBuilder_CanBuild_AnApplication()
		{
			ApplicationBuilder.PayLater.New(_account).Build();
		}
    }
}