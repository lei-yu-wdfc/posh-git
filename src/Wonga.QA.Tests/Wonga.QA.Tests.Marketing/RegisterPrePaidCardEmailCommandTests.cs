using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Api.Requests.Marketing.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Marketing
{
	[Parallelizable(TestScope.All)]
	public class RegisterPrePaidCardEmailCommandTests
	{
		private const string ErrorMarketingEmailIsEmpty = "Marketing_Email_Is_Empty";
		private const string ErrorOpsRequestXmlInvalid = "Ops_RequestXmlInvalid";
		private const string ErrorMarketingEmailHasInvalidFormat = "Marketing_Email_Has_Invalid_Format";
		private const string ErrorMarketingEmailAlreadyExists = "Marketing_Email_Already_Exists";
		
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

			var registeredEmail = Do.Until(() => _marketingDb.RegisteredPrePaidCardEmails.FindByEmail(newEmail));

			Assert.AreEqual(newEmail, registeredEmail.Email);
		}

		[Test, AUT(AUT.Ca), JIRA("CA-2266")]
		public void RegisterPrePaidCardEmailCommandShouldFailWithEmptyEmail()
		{
			var command = new RegisterPrePaidCardEmailCommand
			{
				Email = string.Empty
			};

			try
			{
				Drive.Api.Commands.Post(command);
				Assert.Fail("Invalid command should fault");
			}
			catch(ValidatorException e)
			{
				Assert.IsTrue(e.Errors.Any(error => error == ErrorMarketingEmailIsEmpty));
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-2266")]
		public void RegisterPrePaidCardEmailCommandShouldFailWithNullEmail()
		{
			var command = new RegisterPrePaidCardEmailCommand
			{
				Email = null
			};

			try
			{
				Drive.Api.Commands.Post(command);
				Assert.Fail("Invalid command should fault");
			}
			catch (ValidatorException e)
			{
				Assert.IsTrue(e.Errors.Any(error => error == ErrorOpsRequestXmlInvalid));
			}
		}

		[Test, AUT(AUT.Ca), JIRA("CA-2266")]
		[Row("invalid.email")]
		[Row("invalid@email*.com")]
		[Row("invalid@@email.com")]
		public void RegisterPrePaidCardEmailCommandShouldFailWithInvalidEmail(string invalidEmail)
		{
			var command = new RegisterPrePaidCardEmailCommand
			{
				Email = invalidEmail
			};

			try
			{
				Drive.Api.Commands.Post(command);
				Assert.Fail("Invalid command should fault");
			}
			catch (ValidatorException e)
			{
				Assert.IsTrue(e.Errors.Any(error => error == ErrorMarketingEmailHasInvalidFormat));
			}
		}


		[Test, AUT(AUT.Ca), JIRA("CA-2266")]
		public void RegisterPrePaidCardEmailCommandShouldFailForExistingEmail()
		{
			string existingEmail = Get.GetEmail(50);
			_marketingDb.RegisteredPrePaidCardEmails.UpsertByEmail(Email: existingEmail, CreatedOn: DateTime.UtcNow);

			var command = new RegisterPrePaidCardEmailCommand
			{
				Email = existingEmail
			};

			try
			{
				Drive.Api.Commands.Post(command);
				Assert.Fail("Invalid command should fault");
			}
			catch (ValidatorException e)
			{
				Assert.IsTrue(e.Errors.Any(error => error == ErrorMarketingEmailAlreadyExists));
			}
		}

	}
}
