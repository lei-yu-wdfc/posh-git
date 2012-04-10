using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	public class CheckpointRepaymentPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTRepaymentPredictionPositive;

		private string _forename;
		private string _surname;
		private Date _dateOfBirth;
		private string _nationalNumber;

		private readonly int ScoreCutoffNewUsers = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreNewUsersCutOff").Value);
		private readonly int ScoreCutoffExistingUsers = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreExistingUsersCutOff").Value);
		
		private string _expectedScorecardNameL0;
		private string _expectedScorecardNameLn;
		private int _expectedScoreCutoffNewUsers;
		private int _expectedScoreCutoffExistingUsers;

		private const string ScorecardNameL0Za = "ZARiskScorecard_v_1_2_L0";
		private const string ScorecardNameLnZa = "v_1_0_Ln";
		private const int ScoreCutoffNewUsersZa = 600;
		private const int ScoreCutoffExistingUsersZa = 610;


		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_expectedScorecardNameL0 = ScorecardNameL0Za;
						_expectedScorecardNameLn = ScorecardNameLnZa;
						_expectedScoreCutoffNewUsers = ScoreCutoffNewUsersZa;
						_expectedScoreCutoffExistingUsers = ScoreCutoffExistingUsersZa;

						_forename = "ANITHA";
						_surname = "ESSACK";
						_dateOfBirth = new Date(new DateTime(1957, 12, 19));
						_nationalNumber = "5712190106083";
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveCorrectScorecardUsedL0()
		{
			var scorecardName = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentModelForNewUsers").Value;
			Assert.AreEqual(_expectedScorecardNameL0, scorecardName);
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveCorrectScorecardUsedLn()
		{
			var scorecardName = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentModelForExistingUsers").Value;
			Assert.AreEqual(_expectedScorecardNameLn, scorecardName);
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveCorrectCutoffL0()
		{
			Assert.AreEqual(_expectedScoreCutoffNewUsers, ScoreCutoffNewUsers);
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveCorrectCutoffLn()
		{
			Assert.AreEqual(_expectedScoreCutoffExistingUsers, ScoreCutoffExistingUsers);
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveL0Accept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			var application = ApplicationBuilder.New(customer).WithLoanTerm(5).WithLoanAmount(100).Build();

			var repaymentPredictionScore = GetRepaymentPredictionScore(application);
			Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffNewUsers);
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveL0Decline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(30).WithLoanAmount(2000).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			var repaymentPredictionScore = GetRepaymentPredictionScore(application);
			Assert.LessThan(repaymentPredictionScore, ScoreCutoffNewUsers);
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveLnAccept()
		{
			var customer = CustomerBuilder.New().Build();
			var application1 = ApplicationBuilder.New(customer).Build();
			application1.RepayOnDueDate();

			Drive.Db.UpdateEmployerName(customer.Id, Get.EnumToString(TestMask));

			var application = ApplicationBuilder.New(customer).WithLoanTerm(5).WithLoanAmount(100).Build();

			var repaymentPredictionScore = GetRepaymentPredictionScore(application);
			Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffExistingUsers);
		}

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
