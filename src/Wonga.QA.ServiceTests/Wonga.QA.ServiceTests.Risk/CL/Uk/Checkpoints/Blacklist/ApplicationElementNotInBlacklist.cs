using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
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
						 responsibleVerification: "ApplicantIsNotOnBlackListVerification");
		}


	}
}
