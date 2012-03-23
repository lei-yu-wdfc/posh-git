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

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CheckpointReputationPredictionPositiveTests
	{
		private const RiskMask TestMask = RiskMask.TESTReputationtPredictionPositive;

		private const int ReputationScoreCutoff = 200; //TODO Hardcoded in Risk for now
		private static readonly string[] ExpectedFactorNames = new string[] { "PostcodeInArrears", "LoanNumber", "DeviceCountPostcode", "DeviceDeclineRate" };

		private string _forename;
		private string _surname;
		private Date _dateOfBirth;
		private string _nationalNumber;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_forename = "ANITHA";
						_surname = "ESSACK";
						_dateOfBirth = new Date(new DateTime(1957, 12, 19));
						_nationalNumber = "5712190106083";

						Drive.Db.SetServiceConfiguration("FeatureSwitch.ZA.ReputationPredictionCheckpoint", "true");
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938"), Pending("Work in progress")]
		public void CheckpointReputationPredictionPositiveAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1938"), Pending("Work in progress")]
		public void CheckpointReputationPredictionPositiveDecline()
		{

		}

		[Test, AUT(AUT.Za)]
		public void CheckpointReputationPredictionPositiveCorrectFactorsUsed()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

			var db = new DbDriver();
			var actualFactorNames = (from ra in db.Risk.RiskApplications
										where ra.ApplicationId == application.Id
										join pf in db.Risk.PmmlFactors on ra.RiskApplicationId equals pf.RiskApplicationId
										join f in db.Risk.Factors on pf.FactorId equals f.FactorId
									select f.Name).ToArray();

			Assert.AreElementsEqualIgnoringOrder(ExpectedFactorNames, actualFactorNames);
		}
	}
}