using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Risk.Business;
using Wonga.QA.ServiceTests.Risk.Mocks;

namespace Wonga.QA.ServiceTests.Risk
{
	public class MocksSelfTests
	{
		   private EndpointMock _salesforce;
		[SetUp]
		public void Setup()
		{
			_salesforce = new EndpointMock("salesforcecomponent");
			_salesforce.Start();
		}


		[Test]
		public void ThisMessageIsSentToSalesforceTest()
		{
			var appId = new Guid("00000000-0000-0000-0000-000000000000");

			var thisMessageWasSentToSalesforce = false;

			_salesforce.AddHandler<IBusinessApplicationAccepted>(

			filter: x => x.ApplicationId == appId,
			action: (x, bus) => thisMessageWasSentToSalesforce = true);

			Do.Until(() => thisMessageWasSentToSalesforce);
		}
	}
}
