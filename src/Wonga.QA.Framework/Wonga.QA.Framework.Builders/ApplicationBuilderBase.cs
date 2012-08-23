using System;
using Wonga.QA.Framework.Account;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Builders
{
	public abstract class ApplicationBuilderBase
	{
		protected AccountBase Account { get; private set; }
		protected ApplicationDecisionStatus? ExpectedDecision { get; private set; }
		protected Boolean SignIfAccepted { get; private set; }
		protected IovationMockResponse IovationResponse { get; private set; }

		protected ApplicationBuilderBase(AccountBase account)
		{
			Account = account;
			ExpectedDecision = ApplicationDecisionStatus.Accepted;
			SignIfAccepted = true;
			IovationResponse = IovationMockResponse.Allow;
		}


		#region With Methods

		public ApplicationBuilderBase WithExpectedDecision(ApplicationDecisionStatus decision)
		{
			ExpectedDecision = decision;
			return this;
		}

		public ApplicationBuilderBase WithNoExpectedDecision()
		{
			ExpectedDecision = null;
			return this;
		}

		public ApplicationBuilderBase WithoutSigning()
		{
			SignIfAccepted = false;
			return this;
		}

		public ApplicationBuilderBase WithIovationResponse(IovationMockResponse response)
		{
			IovationResponse = response;
			return this;
		}

		#endregion
	}
}
