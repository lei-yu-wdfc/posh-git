using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Enums;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	class CheckpointRepaymentPredictionPositiveTests
	{
		private const string TestMask = "test:RepaymentPredictionPositive";
		private readonly int ScoreCutoffNewUsers = Int32.Parse(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreNewUsersCutOff").Value);
		private readonly int ScoreCutoffExistingUsers = Int32.Parse(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreExistingUsersCutOff").Value);


		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveL0Accept()
		{
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveL0Decline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			var application =
				ApplicationBuilder.New(customer)
				.Build();

			var db = new DbDriver();
			var repaymentPredictionScore = (from ra in db.Risk.RiskApplications
											join dd in db.Risk.RiskDecisionDatas
												on ra.RiskApplicationId equals dd.RiskApplicationId
											where ra.ApplicationId == application.Id
											select dd.ValueDouble).First();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveLnAccept()
		{
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveLnDecline()
		{
		}


		#region Helpers

		#endregion
	}
}
