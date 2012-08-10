using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ucg
{
	[TestFixture, Parallelizable(TestScope.All), AUT(AUT.Uk, AUT.Ca, AUT.Za)]
	public class ApiCommandTests
	{
		[Test]
		public void CreateAccountCommandTest()
		{
			var accountId = Guid.NewGuid();

			Drive.Ucg.Commands.Post(new CreateAccountCommand
			                        	{AccountId = accountId, Login = Get.RandomEmail(), Password = Get.GetPassword()});

			Do.Until(() => Drive.Data.Ops.Db.Accounts.FindByExternalId(accountId));
		}
	}
}
