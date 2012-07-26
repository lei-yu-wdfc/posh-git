using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Risk.BlackList;
using Wonga.QA.ServiceTests.Risk.CL.uk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.CL.Uk.Checkpoints.Blacklist
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	public class ApplicationElementNotInBlacklist : RiskServiceTestClUkBase
	{
		[Test]
		public void IfMainApplicantFoundOnBlackList_ApplicationIsDeclined()
		{
			GivenThatApplicantIsOnBlackList();
			WhenTheL0UserAppliesForALoan();
			ThenTheRiskServiceShouldDeclineTheLoan();
		}

		[Test]
		public void IfApplicationIsDeclined_ApplicationDeclinedIsPublished()
		{
			ExpectingRiskToPublishApplicationDeclined();
			
			GivenThatApplicantIsOnBlackList();
			WhenTheL0UserAppliesForALoan();
			RiskPublishesApplicationDeclinedEvent();
		}

		private void RiskPublishesApplicationDeclinedEvent()
		{
			Do.Until(() =>_eventPublished);
		}

		private bool _eventPublished = false;
		private void ExpectingRiskToPublishApplicationDeclined()
		{
			EndpointMock.Subscribe<Wonga.QA.Framework.Msmq.Messages.Risk.IApplicationDeclined>(
				x => x.ApplicationId == ApplicationId);

			EndpointMock.AddHandler<Wonga.QA.Framework.Msmq.Messages.Risk.IApplicationDeclined>(
				x => x.ApplicationId == ApplicationId,
				(x, bus) => _eventPublished = true);
		}


		private void GivenThatApplicantIsOnBlackList()
		{
			EndpointMock.AddHandler<ConsumerBlackListRequestMessage>(
						filter: x => x.ApplicationId == this.ApplicationId,
						action: (receivedMsg, bus) =>
						{
							var response = CreateBlacklistedMessage();
							response.SagaId = receivedMsg.SagaId;
							Send(response);
						});
		}

		private ConsumerBlackListResponseMessage CreateBlacklistedMessage()
		{
			return new ConsumerBlackListResponseMessage {PresentInBlackList = true};
		}

		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();

			Background(maskName: RiskMask.TESTBlacklist,
						 checkpointName: "ApplicationElementNotOnBlacklist",
						 responsibleVerification: "ApplicantIsNotOnBlackListVerification");
		}


	}
}
