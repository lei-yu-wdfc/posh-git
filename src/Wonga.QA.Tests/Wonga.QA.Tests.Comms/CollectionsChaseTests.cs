using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CollectionsChaseTests
	{

		[Test, Explicit("Release")]
		public void CollectionsChaseEmails()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears();

		}


	}
}
