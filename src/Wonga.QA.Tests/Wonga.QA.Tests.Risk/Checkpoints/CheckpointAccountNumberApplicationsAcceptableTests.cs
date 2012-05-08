using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	public class CheckpointAccountNumberApplicationsAcceptableTests
	{
		private const RiskMask TestMask = RiskMask.TESTAccountNumberApplicationsAcceptable;

		private const int DailyThreshold = 5;
		private const int ThirtyDayThreshold = 15;

		private Customer customer;
		private Application application;

		[Test, AUT(AUT.Za), JIRA("ZA-2228")]
		public void L0_NumberOfApplicationsBelowThresholdAccepted()
		{
			customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			application = ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2228"), DependsOn("L0_NumberOfApplicationsBelowThresholdAccepted")]
		public void Ln_NumberOfApplicationsBelowThresholdAccepted()
		{
			application.RepayOnDueDate();
			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2228"), Timeout(0)]
		public void Ln_NumberOfApplicationDailyOverThresholdDeclined()
		{
			var customer2 = CustomerBuilder.New().WithEmployer(TestMask).Build();

			for( int i = 0; i < DailyThreshold; i++)
			{
				ApplicationBuilder.New(customer2).Build().RepayOnDueDate();
			}

			ApplicationBuilder.New(customer2).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}
	}
}
