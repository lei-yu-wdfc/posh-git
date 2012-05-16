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

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-2228", "CA-1879")]
		public void L0_NumberOfApplicationsBelowThresholdAccepted()
		{
			customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			application = ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-2228", "CA-1879"), DependsOn("L0_NumberOfApplicationsBelowThresholdAccepted")]
		public void Ln_NumberOfApplicationsBelowThresholdAccepted()
		{
			application.RepayOnDueDate();
			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za, AUT.Ca), JIRA("ZA-2228", "CA-1879"), Timeout(0)]
		public void Ln_NumberOfApplicationDailyOverThresholdDeclined()
		{
			var customer2 = CustomerBuilder.New().WithEmployer(RiskMask.TESTEmployedMask).WithEmployerStatus("Unemployed").Build();
			//var customer2 = CustomerBuilder.New().WithEmployer(RiskMask.TESTNoCheck).Build();

			for( int i = 0; i < DailyThreshold; i++)
			{
				ApplicationBuilder.New(customer2).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
				//ApplicationBuilder.New(customer2).Build();
			}

			customer2.UpdateEmployer(TestMask.ToString());

			ApplicationBuilder.New(customer2).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}
	}
}
