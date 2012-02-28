using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Enums;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	class CheckpointRepaymentPredictionPositiveTests
	{
		private const string TestMask = "test:RepaymentPredictionPositive";
		private readonly int ScoreCutoffNewUsers = Int32.Parse(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreNewUsersCutOff").Value);
		private readonly int ScoreCutoffExistingUsers = Int32.Parse(Driver.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreExistingUsersCutOff").Value);


		//[Test, AUT(AUT.Za)]
		//public void CheckpointRepaymentPredictionPositiveL0Accept()
		//{
		//    var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
		//    var application = ApplicationBuilder.New(customer).Build();

		//    var repaymentPredictionScore = GetRepaymentPredictionScore(application);
		//    Assert.GreaterThan(ScoreCutoffNewUsers, repaymentPredictionScore);
		//}

		//[Test, AUT(AUT.Za)]
		//public void CheckpointRepaymentPredictionPositiveL0Decline()
		//{
		//    var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
		//    var application = ApplicationBuilder.New(customer).WithLoanTerm(30).WithLoanAmount(2000).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();

		//    var repaymentPredictionScore = GetRepaymentPredictionScore(application);
		//    Assert.LessThan(ScoreCutoffNewUsers, repaymentPredictionScore);
		//}

		//[Test, AUT(AUT.Za)]
		//public void CheckpointRepaymentPredictionPositiveLnAccept()
		//{
		//    var customer = CustomerBuilder.New().WithEmployer("test:IsEmployed").Build();
		//    ApplicationBuilder.New(customer).Build().RepayOnDueDate();

		//}

		//[Test, AUT(AUT.Za)]
		//public void CheckpointRepaymentPredictionPositiveLnDecline()
		//{
		//}


		#region Helpers

		private double GetRepaymentPredictionScore(Application application)
		{
			var db = new DbDriver();
			return (double)(from ra in db.Risk.RiskApplications
					join dd in db.Risk.RiskDecisionDatas
						on ra.RiskApplicationId equals dd.RiskApplicationId
					where ra.ApplicationId == application.Id
					select dd.ValueDouble).First();
		}

		private void AssertFactorsStoredInTable(Application application)
		{
			
		}

		#endregion
	}
}
