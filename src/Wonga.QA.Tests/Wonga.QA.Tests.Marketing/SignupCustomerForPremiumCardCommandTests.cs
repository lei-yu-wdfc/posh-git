using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api.Requests.Payments.PrepaidCard.Commands;

namespace Wonga.QA.Tests.Marketing
{
	[Parallelizable(TestScope.All)]
	class SignupCustomerForPremiumCardCommandTests
	{
		[Test, AUT(AUT.Ca)]
		[Ignore]
		public void TestPlaceHolder()
		{
			var command = new SignupCustomerForPremiumCardCommand
			              	{
			              		CustomerExternalId = Guid.NewGuid()
			              	};

			Drive.Api.Commands.Post(command);
		}
	}
}
