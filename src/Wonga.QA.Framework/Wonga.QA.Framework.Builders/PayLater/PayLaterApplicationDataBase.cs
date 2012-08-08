using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public class PayLaterApplicationDataBase
	{
		public ApplicationDecisionStatus? ExpectedDecision;

		public PayLaterApplicationDataBase()
		{
			ExpectedDecision = ApplicationDecisionStatus.Accepted;
		}
	}
}
