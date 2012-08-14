using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Ucg
{
	[TestFixture, Parallelizable(TestScope.All), AUT(AUT.Ca)]
	public class UcgApiQueryTests
	{
		[Test]
		public void GetAccountQueryTest()
		{
			string login = Get.RandomEmail();
			string password = Get.GetPassword();
			Guid accountId = Guid.NewGuid();

			Drive.Ucg.Commands.Post(new CreateAccountCommand { AccountId = accountId, Login = login, Password = password });

			Do.Until(() => Drive.Data.Ops.Db.Accounts.FindByExternalId(accountId));
			
			ApiResponse response = Drive.Ucg.Queries.Post(new GetAccountQuery{ Login = login, Password = password });

			Assert.AreEqual(accountId.ToString(), response.Values["AccountId"].Single());
		}
	}
}
