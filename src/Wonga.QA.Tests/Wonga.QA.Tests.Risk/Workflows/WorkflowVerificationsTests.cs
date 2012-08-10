﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Checkpoints;

namespace Wonga.QA.Tests.Risk.Workflows
{
	[Parallelizable(TestScope.All), Category(TestCategories.CoreTest)]
    [AUT(AUT.Ca, AUT.Za)]
	public class WorkflowVerificationsTests
	{
		private readonly List<string> ExpectedVerificationNamesL0 = GetExpectedVerificationNamesL0();
		private readonly List<string> ExpectedVerificationNamesLn = GetExpectedVerificationNamesLn();

		#region Ca

		private static readonly List<string> ExpectedVerificationNamesL0Ca = new List<string>
		                                                                     	{
																					"AccountNumberApplicationsAcceptableVerification",
		                                                                     		"CustomerIsEmployedVerification",
																					"CustomerIsNotOnBenefitsVerification",
		                                                                     		"ApplicantIsNotMinorVerification",
		                                                                     		"MonthlyIncomeBCVerification",
		                                                                     		"MonthlyIncomeVerification",
		                                                                     		"MobilePhoneIsUniqueVerification",
		                                                                     		"HomePhoneIsAcceptableVerification",
		                                                                     		"BlackListVerification",
		                                                                     		"IovationAutoReviewVerification",
		                                                                     		"IovationVerification",
		                                                                     		"CreditBureauDataIsAvailableVerification",
		                                                                     		"CreditBureauCustomerIsSolventVerification",
		                                                                     		"CreditBureauCustomerIsAliveVerification",
		                                                                     		"DateOfBirthIsCorrectVerification",
		                                                                     		"RepaymentPredictionPositiveVerification",
		                                                                     		"DirectFraudCheckVerification",
																					"DoNotRelendVerification", 
																					"ReputationPredictionPositiveVerification",
		                                                                     	};

		private static readonly List<string> ExpectedVerificationNamesLnCa = new List<string>
		                                                                     	{
																					"CustomerIsEmployedVerification",
		                                                                     		"CustomerIsNotOnBenefitsVerification",
		                                                                     		"ApplicantIsNotMinorVerification",
		                                                                     		"MonthlyIncomeBCVerification",
		                                                                     		"MonthlyIncomeVerification",
		                                                                     		"BlackListVerification",
		                                                                     		"IovationAutoReviewVerification",
		                                                                     		"IovationVerification",
		                                                                     		"CreditBureauDataIsAvailableVerification",
		                                                                     		"CreditBureauCustomerIsSolventVerification",
		                                                                     		"CreditBureauCustomerIsAliveVerification",
		                                                                     		"DateOfBirthIsCorrectVerification",
		                                                                     		"CreditBureauScoreIsAcceptableVerification",
		                                                                     		"DirectFraudCheckVerification",
																					"DoNotRelendVerification", 
		                                                                     	};

		#endregion

		#region Za

		private static readonly List<string> ExpectedVerificationNamesL0Za = new List<string>()
		                                                             	{
																			"AccountNumberApplicationsAcceptableVerification",
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
		                                                             		"CustomerIsSolventNoticeVerification",
		                                                             		"CustomerIsSolventVerification",
		                                                             		"CustomerIsAliveVerification",
		                                                             		"RepaymentPredictionVerification",
		                                                             		"BankAccountIsValidVerification"
		                                                             	};

		private static readonly List<string> ExpectedVerificationNamesLnZa = new List<string>()
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
		                                                             		"CustomerIsSolventNoticeVerification",
		                                                             		"CustomerIsSolventVerification",
		                                                             		"CustomerIsAliveVerification",
		                                                             		"RepaymentPredictionVerification",
		                                                             		"BankAccountIsValidVerification"
		                                                             	};

		#endregion


		[Test, AUT(AUT.Ca, AUT.Za)]
		public void WorkflowCorrectVerificationsUsedL0()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
				.WithoutExpectedDecision()
				.Build();

			var actualVerificationNames = Drive.Db.GetVerificationDefinitionsForApplication(application.Id).Select(a => a.Name);
			Assert.AreElementsEqualIgnoringOrder(ExpectedVerificationNamesL0, actualVerificationNames);
		}

		[Test, AUT(AUT.Ca, AUT.Za)]
		public void WorkflowCorrectVerificationsUsedLn()
		{
			var customer = CustomerBuilder.New().Build();
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();
            CustomerOperations.UpdateEmployerNameInRisk(customer.Id, "Wonga");

			var application =
				ApplicationBuilder.New(customer)
				.WithoutExpectedDecision()
				.Build();

			var actualVerificationNames = Drive.Db.GetVerificationDefinitionsForApplication(application.Id).Select(a => a.Name);
			Assert.AreElementsEqualIgnoringOrder(ExpectedVerificationNamesLn, actualVerificationNames);
		}

		#region Helpers

		private static List<string> GetExpectedVerificationNamesL0()
		{
			switch (Config.AUT)
			{
				case( AUT.Ca):
					{
						return ExpectedVerificationNamesL0Ca;
					}
				case AUT.Za:
					{
						return ExpectedVerificationNamesL0Za;
					}

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
				case AUT.Ca:
				{
					return ExpectedVerificationNamesLnCa;
				}
				case AUT.Za:
				{
					return ExpectedVerificationNamesLnZa;
				}
				default:
				{
					return new List<string>();
					//throw new NotImplementedException(Config.AUT.ToString());
				}
			}
		}
		#endregion
	}
}
