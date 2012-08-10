using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Cs
{
	[TestFixture, Parallelizable(TestScope.All), AUT(AUT.Ca)]
	public class CancelApplicationTests
	{
		[Test]
		public void CancelNonExistentApplication()
		{
			var command = new CancelApplicationCommand { AccountId = Guid.NewGuid(), ApplicationId = Guid.NewGuid() };
				
			try
			{
				Drive.Cs.Commands.Post(command);
			}
			catch (ValidatorException e)
			{
				Assert.IsTrue(e.Errors.Contains("Payments_Application_NotFound"));
			}
			
		}

		[Test]
		public void CancelSignedApplication()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();

			var command = new CancelApplicationCommand { AccountId = customer.Id, ApplicationId = application.Id };

			try
			{
				Drive.Cs.Commands.Post(command);
			}
			catch (ValidatorException e)
			{
				Assert.IsTrue(e.Errors.Contains("Payments_Application_IsSigned"));
			}
			
		}

	}
}
