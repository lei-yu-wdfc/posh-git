using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.ServiceTests.Risk
{
	public abstract class RiskServiceTestBase : ServiceTestBase
	{
		protected void AssertVerificationStarted()
		{
			AssertApplicationDecisionIs(ApplicationDecisionStatus.Pending);
		}
		//TODO[seb]: this is a duplication from QAF application builder. Move all messages and this to a custom QAF builder
		private void AssertApplicationDecisionIs(ApplicationDecisionStatus expectedDecision)
		{
			Do.With.Timeout(2).Message("Risk didn't return expected status \"{0}\"", expectedDecision).Until(
				() =>
				(ApplicationDecisionStatus)
				Enum.Parse(typeof (ApplicationDecisionStatus),
				           Drive.Api.Queries.Post(new GetApplicationDecisionQuery {ApplicationId = ApplicationId}).Values[
				           	"ApplicationDecisionStatus"].Single()) == expectedDecision);
		}
	}
}