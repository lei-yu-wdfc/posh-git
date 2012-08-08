using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ucg
{
	[TestFixture, AUT(AUT.Uk)]
	public class ApiCommandTests
	{

		[Test]
		public void CreateAccountCommandTest()
		{
			Drive.Ucg.Commands.Post(new CreateAccountCommand
			                        	{AccountId = Guid.NewGuid(), Login = Get.GetEmail(), Password = Get.GetPassword()});
		}
	}
}
