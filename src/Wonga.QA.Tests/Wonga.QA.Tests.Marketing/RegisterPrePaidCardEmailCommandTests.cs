using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Marketing
{
	[Parallelizable(TestScope.All)]
	public class RegisterPrePaidCardEmailCommandTests
	{
		private static readonly dynamic _marketingDb = Drive.Data.Marketing.Db;

		[Test, AUT(AUT.Ca), JIRA("CA-2266")]
		public void RegisterPrePaidCardEmailCommandShouldSaveEmailInDatabase()
		{
			string newEmail = Get.GetEmail(50);

			var command = new RegisterPrePaidCardEmailCommand
			              	{
								Email = newEmail
			              	};

			Drive.Api.Commands.Post(command);

			var registereEmail = Do.Until(() => _marketingDb.RegisteredPrePaidCardEmails.FindByEmail(newEmail));

			Assert.IsNull(registereEmail);

		}

		//TODO: test with empty email

		//TODO: test with invalid email

		//TODO: test with existing email

	}
}
