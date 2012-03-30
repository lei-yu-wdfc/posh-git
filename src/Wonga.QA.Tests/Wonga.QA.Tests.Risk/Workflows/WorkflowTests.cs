using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Workflows
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class WorkflowTests
	{
		private readonly List<string> ExpectedCheckpointNamesL0 = GetExpectedCheckpointNamesL0();
		private readonly List<string> ExpectedCheckpointNamesLn = GetExpectedCheckpointNamesLn(); 
		private readonly List<string> ExpectedVerificationNamesL0 = GetExpectedVerificationNamesL0();
		private readonly List<string> ExpectedVerificationNamesLn = GetExpectedVerificationNamesLn();

		private static readonly List<string> ExpectedCheckpointNamesL0Za = new List<string>()
		                                                         	{
		                                                         		"Customer has provided correct forename & surname",
		                                                         		"Mobile phone is unique",
		                                                         		"Reputation prediction check",
		                                                         		"Customer is employed",
		                                                         		"Monthly income limit check",
		                                                         		"Application terms are acceptable for business",
		                                                         		"Hardware blacklist check",
		                                                         		"Application data blacklist check",
		                                                         		"Credit bureau data is available",
		                                                         		"Customer is solvent",
		                                                         		"Applicant is alive",
		                                                         		"Repayment prediction check",
		                                                         		"Bank account is valid",
																		"Fraud list check"
		                                                         	};

		private static readonly List<string> ExpectedCheckpojeintNamesLnZa = new List<string>()
		                                                                    	{
		                                                                    		"Customer is employed",
		                                                                    		"Monthly income limit check",
		                                                                    		"Application terms are acceptable for business",
		                                                                    		"Hardware blacklist check",
		                                                                    		"Application data blacklist check",
		                                                                    		"Fraud list check",
		                                                                    		"Credit bureau data is available",
		                                                                    		"Customer is solvent",
		                                                                    		"Applicant is alive",
		                                                                    		"Repayment prediction check",
		                                                                    		"Bank account is valid"
		                                                                    	};

		private static List<string> _expectedVerificationNamesL0Za = new List<string>()
		                                                             	{
		                                                             		"CustomerIsEmployedVerification",
		                                                             		"MonthlyIncomeVerification",
		                                                             		"ApplicationTermNotLessThan4DaysVerification",
		                                                             		"MobilePhoneIsUniqueVerification",
		                                                             		"BlackListVerification",
		                                                             		"IovationAutoReviewVerification",
		                                                             		"IovationVerification",
		                                                             		"DoNotRelendVerification",
		                                                             		"FraudBlacklistVerification",
		                                                             		"ReputationPredictionVerification",
		                                                             		"CreditBureauDataIsAvailableVerification",
		                                                             		"CustomerNameIsCorrectVerification",
		                                                             		"ApplicantIsSolventNoticeVerification",
		                                                             		"ApplicantIsSolventVerification",
		                                                             		"CustomerIsAliveVerification",
		                                                             		"RepaymentPredictionVerification",
		                                                             		"BankAccountIsValidVerification"
		                                                             	};

		private static List<string> _expectedVerificationNamesLnZa = new List<string>()
		                                                             	{
		                                                             		"CustomerIsEmployedVerification",
		                                                             		"MonthlyIncomeVerification",
		                                                             		"ApplicationTermNotLessThan4DaysVerification",
		                                                             		"BlackListVerification",
		                                                             		"IovationAutoReviewVerification",
		                                                             		"IovationVerification",
		                                                             		"DoNotRelendVerification",
		                                                             		"FraudBlacklistVerification",
		                                                             		"CreditBureauDataIsAvailableVerification",
		                                                             		"ApplicantIsSolventNoticeVerification",
		                                                             		"ApplicantIsSolventVerification",
		                                                             		"CustomerIsAliveVerification",
		                                                             		"RepaymentPredictionVerification",
		                                                             		"BankAccountIsValidVerification"
		                                                             	};

		[Test, AUT(AUT.Za), Explicit]
		public void WorkflowL0SingleWorkflowUsed()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			var workflows = Drive.Db.GetWorkflowsForApplication(application.Id);

			Assert.AreEqual(1, workflows.Count);
		}

		[Test, AUT(AUT.Za)]
		public void WorkflowCorrectCheckpointsUsedL0()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
                .WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			var actualCheckpointNames = Drive.Db.GetCheckpointDefinitionsForApplication(application.Id).Select(a => a.Name);
			Assert.AreElementsEqualIgnoringOrder(ExpectedCheckpointNamesL0, actualCheckpointNames);
		}

		[Test, AUT(AUT.Za)]
		public void WorkflowCorrectCheckpointsUsedLn()
		{
			var customer = CustomerBuilder.New().Build();
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			Drive.Db.UpdateEmployerName(customer.Id, "Wonga");

			var application =
				ApplicationBuilder.New(customer)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			var actualCheckpointNames = Drive.Db.GetCheckpointDefinitionsForApplication(application.Id).Select(a => a.Name);
			Assert.AreElementsEqualIgnoringOrder(ExpectedCheckpointNamesLn, actualCheckpointNames);
		}

		[Test, AUT(AUT.Za)]
		public void WorkflowCorrectVerificationsUsedL0()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			var actualVerificationNames = Drive.Db.GetVerificationDefinitionsForApplication(application.Id).Select(a => a.Name);
			Assert.AreElementsEqual(ExpectedVerificationNamesL0, actualVerificationNames);
		}

		[Test, AUT(AUT.Za)]
		public void WorkflowCorrectVerificationsUsedLn()
		{
			var customer = CustomerBuilder.New().Build();
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			Drive.Db.UpdateEmployerName(customer.Id, "Wonga");

			var application =
				ApplicationBuilder.New(customer)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			var actualVerificationNames = Drive.Db.GetVerificationDefinitionsForApplication(application.Id).Select(a => a.Name);
			Assert.AreElementsEqual(ExpectedVerificationNamesLn, actualVerificationNames);
		}

		#region Helpers

		private static List<string> GetExpectedCheckpointNamesL0()
		{
			switch (Config.AUT)
			{
					case AUT.Za:
					{
						return ExpectedCheckpointNamesL0Za;
					}
				    default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		private static List<string> GetExpectedCheckpointNamesLn()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						return ExpectedCheckpointNamesLnZa;
					}
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		private static List<string> GetExpectedVerificationNamesL0()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						return _expectedVerificationNamesL0Za;
					}
					break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		private static List<string> GetExpectedVerificationNamesLn()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						return _expectedVerificationNamesLnZa;
					}
					break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}
		#endregion
	}
}
