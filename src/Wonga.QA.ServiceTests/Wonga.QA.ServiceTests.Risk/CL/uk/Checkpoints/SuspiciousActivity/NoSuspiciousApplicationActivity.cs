using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.ServiceTests.Risk.CL.uk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.CL.Uk.Checkpoints.SuspiciousActivity
{
	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	class NoSuspiciousApplicationActivity : RiskServiceTestClUkBase
	{
		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();
			Background(maskName: RiskMask.TESTNoSuspiciousApplicationActivity,
						 checkpointName: "NoSuspiciousApplicationActivity",
						 responsibleVerification: "SuspiciousActivityVerification");
		}

		[Test]
		public void IfMainApplicantFoundOnBlackList_ApplicationIsDeclined()
		{
			WhenTheL0UserAppliesForALoan();
			ThenTheRiskServiceShouldDeclineTheLoan();
		}
	}
}
