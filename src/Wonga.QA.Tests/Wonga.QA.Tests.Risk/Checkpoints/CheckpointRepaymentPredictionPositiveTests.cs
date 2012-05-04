using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	public class CheckpointRepaymentPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTRepaymentPredictionPositive;

		private Customer customerLn;
		private Application applicationL0;
		private Application applicationLn;

		private string _forename;
		private string _surname;
		private Date _dateOfBirth;
		private string _nationalNumber;
		
		#region ScorecardNames

		private string _expectedScorecardNameL0;
		private string _expectedScorecardNameLn;
		private const string ScorecardNameL0Za = "ZARiskScorecard_v_1_4_L0";
		private const string ScorecardNameLnZa = "ZARiskScorecard_v_1_2_LN";

		#endregion

		#region Cutoffs

		private readonly int ScoreCutoffNewUsers = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreNewUsersCutOff").Value);
		private readonly int ScoreCutoffExistingUsers = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Risk.RepaymentScoreExistingUsersCutOff").Value);

		private int _expectedScoreCutoffNewUsers;
		private int _expectedScoreCutoffExistingUsers;
		private const int ScoreCutoffNewUsersZa = 600;
		private const int ScoreCutoffExistingUsersZa = 600;

		#endregion

		#region Factors

		private string[] _expectedFactorNamesL0;
		private string[] _expectedFactorNamesLn;

		//private static readonly string[] ExpectedFactorNamesL0Za = new string[];
		private static readonly string[] FactorNamesLnZa = new string[]
		                                                   	{
		                                                   		"NumAccounts",
		                                                   		"TotalCurBalance",
		                                                   		"OverdueRatio",
		                                                   		"EmpiricaScore",
		                                                   		"LoanAmount",
		                                                   		"LoanTerm",
		                                                   		"LoanNumber",
		                                                   		"AvgDel",
		                                                   		"ArrearsPresent",
		                                                   		"DaysSinceLastLoan",
		                                                   		"AppNetMonthlyIncome"
		                                                   	};
		#endregion

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
						_expectedFactorNamesLn = FactorNamesLnZa;

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
		public void CheckpointRepaymentPredictionPositiveL0Accept()
		{
			var customer = BuildCustomerForScorecardAccept();
			var application = ApplicationBuilder.New(customer).WithLoanTerm(5).WithLoanAmount(100).Build();

			var repaymentPredictionScore = GetRepaymentPredictionScore(application);
			Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffNewUsers);

			customerLn = customer;
			applicationL0 = application;
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointRepaymentPredictionPositiveL0Decline()
		{
			var customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(30).WithLoanAmount(2000).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			var repaymentPredictionScore = GetRepaymentPredictionScore(application);
			Assert.LessThan(repaymentPredictionScore, ScoreCutoffNewUsers);
		}

		[Test, AUT(AUT.Za), DependsOn("CheckpointRepaymentPredictionPositiveL0Accept")]
		public void CheckpointRepaymentPredicitionPositiveFactorsUsedAreCorrectL0()
		{
			var factorNames = GetFactors(applicationL0);
			Assert.AreElementsEqualIgnoringOrder(_expectedFactorNamesL0, factorNames);
		}

		[Test, AUT(AUT.Za), DependsOn("CheckpointRepaymentPredictionPositiveL0Accept")]
		public void CheckpointRepaymentPredictionPositiveLnAccept()
		{
			var application = ApplicationBuilder.New(customerLn).WithLoanTerm(5).WithLoanAmount(100).Build();

			var repaymentPredictionScore = GetRepaymentPredictionScore(application);
			Assert.GreaterThan(repaymentPredictionScore, ScoreCutoffExistingUsers);

			applicationLn = application;
		}

		[Test, AUT(AUT.Za), DependsOn("CheckpointRepaymentPredictionPositiveLnAccept")]
		public void CheckpointRepaymentPredicitionPositiveFactorsUsedAreCorrectLn()
		{
			var factorNames = GetFactors(applicationLn);
			Assert.AreElementsEqualIgnoringOrder(_expectedFactorNamesLn, factorNames);
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

		#region Helpers

		private Customer BuildCustomerForScorecardAccept()
		{
			return CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();
		}

		private double GetRepaymentPredictionScore(Application application)
		{
			var riskWorkflowId = (int)Do.Until(() => Drive.Data.Risk.Db.RiskWorkflows.FindByApplicationId(application.Id)).RiskWorkflowId;
			var score = (double)Do.Until(() =>Drive.Data.Risk.Db.RiskDecisionData.FindByRiskWorkflowId(riskWorkflowId).ValueDouble);

			return score;
		}

		private IEnumerable<string> GetFactors(Application application)
		{
			int workflowId = (int)Drive.Data.Risk.Db.RiskWorkflows.FindByApplicationId(application.Id).RiskWorkflowId;

			var pmmlFactorIds = ((List<PmmlFactorEntity>)Drive.Data.Risk.Db.PmmlFactors.FindAllByRiskWorkflowId(workflowId).ToList<PmmlFactorEntity>()).Select(a => a.FactorId);
			var factorNames = ((List<FactorEntity>)Drive.Data.Risk.Db.Factors.FindAllByFactorId(pmmlFactorIds).ToList<FactorEntity>()).Select(a => a.Name).ToList();

			return factorNames;
		}

		#endregion
	}
}
