using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	public static class ApplicationBuilderExtensions
	{
		public static ApplicationBuilder WithUnmaskedExpectedDecision(this ApplicationBuilder builder)
		{
			return builder.WithExpectedDecision(GetUnmaskedExpectedDecision());
		}

		private static ApplicationDecisionStatus GetUnmaskedExpectedDecision()
		{
			switch (Config.SUT)
			{
				case SUT.RC:
				case SUT.RCRelease:
					return ApplicationDecisionStatus.Declined;

				default:
					return ApplicationDecisionStatus.Pending;
			}
		}
	}
}
