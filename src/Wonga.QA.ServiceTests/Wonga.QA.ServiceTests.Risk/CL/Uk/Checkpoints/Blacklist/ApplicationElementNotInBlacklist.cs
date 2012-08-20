using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mocks.Service;
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

		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();
			Background(maskName: RiskMask.TESTBlacklist,
						 checkpointName: "ApplicationElementNotOnBlacklist",
                         responsibleVerification: "IovationVerification");
		}

        protected void GivenThatApplicantIsOnBlackList()
        {
            EndpointMock
                .OnArrivalOf<ConsumerBlackListRequestMessage>()
                .Matching(x => x.ApplicationId == ApplicationId)
                .ThenDoThis((receivedMsg, bus) =>
                {
                    var response = CreateBlacklistedMessage();
                    response.SagaId = receivedMsg.SagaId;
                    Send(response);
                })

                .SeemsLegit().Dude();
        }

	    private ConsumerBlackListResponseMessage CreateBlacklistedMessage()
        {
            return new ConsumerBlackListResponseMessage { PresentInBlackList = true };
        }
	}
}
