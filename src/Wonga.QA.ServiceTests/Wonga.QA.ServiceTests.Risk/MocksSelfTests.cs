using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mocks.Service;
using Wonga.QA.Framework.Msmq.Messages.Risk.Business;

namespace Wonga.QA.ServiceTests.Risk
{
	public class MocksSelfTests
	{
		   private EndpointMock _salesforce;
		[SetUp]
		public void Setup()
		{
			_salesforce = new EndpointMock("servicetest",Drive.Msmq.Risk);
			_salesforce.Start();
		}


		[Test]
		public void ThisMessageIsSentToSalesforceTest()
		{
			var appId = new Guid();

			var thisMessageWasSentToSalesforce = false;

			_salesforce.AddHandler<IBusinessApplicationAccepted>(

			filter: x => x.ApplicationId == appId,
			action: (x) => thisMessageWasSentToSalesforce = true);

			Do.Until(() => thisMessageWasSentToSalesforce);
		}
	}
}
