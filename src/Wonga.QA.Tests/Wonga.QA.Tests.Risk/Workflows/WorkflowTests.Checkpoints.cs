using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Workflows
{
	public partial class WorkflowTests
	{
		#region Checkpoints

		private readonly List<string> ExpectedCheckpointNamesL0 = GetExpectedCheckpointNamesL0();
		private readonly List<string> ExpectedCheckpointNamesLn = GetExpectedCheckpointNamesLn();

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

		private static readonly List<string> ExpectedCheckpointNamesLnZa = new List<string>()
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

		#endregion

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

		#endregion
	}
}
