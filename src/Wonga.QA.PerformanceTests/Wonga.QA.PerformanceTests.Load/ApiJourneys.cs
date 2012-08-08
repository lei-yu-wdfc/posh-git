using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.PerformanceTests.Load
{
	public class ApiJourneys
	{
		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
		public void L0JourneyAccepted()
		{
			new LoadRunner {
				Concurrency = 10,
				Duration = TimeSpan.FromSeconds(600),
				Test = () =>
				{
					Customer cust = CustomerBuilder.New().Build();
					ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
				} }.Run();
		}
	}
}
