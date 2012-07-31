using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Framework.Svc.Mocks;
using Wonga.QA.ServiceTests.Risk.CL.uk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.CL.Uk
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	public class RiskDecisionsPublishingTests : RiskServiceTestClUkBase
	{
		[Test]
		public void IfApplicationIsDeclined_ApplicationDeclinedIsPublished()
		{
			ExpectingRiskToPublishApplicationDeclined();

			GivenThatApplicantIsOnBlackList();
			WhenTheL0UserAppliesForALoan();
			RiskPublishesApplicationDeclinedEvent();
		}

		#region Subscription Setup

		private bool _eventPublished;

		private void ExpectingRiskToPublishApplicationDeclined()
		{
			EndpointMock
				.SubscribeTo<IApplicationDeclined>()
				.Matching(x=> x.ApplicationId == ApplicationId)
				.ThenDoThis(x =>_eventPublished = true)
				
				.SeemsLegit().Dude();
		}

		private void RiskPublishesApplicationDeclinedEvent()
		{
			Do.Until(() => _eventPublished);
		}

		#endregion

	}
}
