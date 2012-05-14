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
		

		private readonly List<string> ExpectedCheckpointNamesL0 = GetExpectedCheckpointNamesL0();
		private readonly List<string> ExpectedCheckpointNamesLn = GetExpectedCheckpointNamesLn();

		#region Za

		private static readonly List<string> ExpectedCheckpointNamesL0Za = new List<string>()
		                                                         	{
																		"Check for excesive number of applications",
		                                                         		"Customer has provided correct forename & surname",
		                                                         		"Mobile phone is unique",
		                                                         		"Reputation prediction check",
		                                                         		"Customer is employed",
		                                                         		"Monthly income limit check",
		                                                         		"Application terms are acceptable for business",
		                                                         		"Hardware blacklist check",
		                                                         		"Application data blacklist check",
		                                                         		"Credit bureau data is available",
		                                                         		"Applicant is solvent",
		                                                         		"Applicant is alive",
		                                                         		"Repayment prediction check",
		                                                         		"Bank account is valid",
																		"Fraud list check"
		                                                         	};

		private static readonly List<string> ExpectedCheckpointNamesLnZa = new List<string>()
		                                                                    	{
																					"Check for excesive number of applications",
		                                                                    		"Customer is employed",
		                                                                    		"Monthly income limit check",
		                                                                    		"Application terms are acceptable for business",
		                                                                    		"Hardware blacklist check",
		                                                                    		"Application data blacklist check",
		                                                                    		"Fraud list check",
		                                                                    		"Credit bureau data is available",
		                                                                    		"Applicant is solvent",
		                                                                    		"Applicant is alive",
		                                                                    		"Repayment prediction check",
		                                                                    		"Bank account is valid"
		                                                                    	};


		#endregion

		#region Ca

		private static readonly List<string> ExpectedCheckpointNamesL0Ca = new List<string>()
		                                                                   	{
		                                                                   		"Mobile phone is unique",
		                                                                   		"Home phone is acceptable",
		                                                                   		"User assisted fraud check",
		                                                                   		"Repayment prediction check",
		                                                                   		"Date of birth is correct",
		                                                                   		"Customer is employed",
		                                                                   		"Monthly income limit check",
		                                                                   		"Application data blacklist check",
		                                                                   		"Hardware blacklist check",
		                                                                   		"Credit bureau data is available",
		                                                                   		"Applicant is not minor",
		                                                                   		"Ability to verify personal data",
		                                                                   		"Applicant is alive",
		                                                                   		"Customer is solvent",
																				"Reputation prediction check", 
																				"Fraud list check"
		                                                                   	};

		private static readonly List<string> ExpectedCheckpointNamesLnCa = new List<string>()
		                                                                   	{
		                                                                   		"User assisted fraud check",
		                                                                   		"Credit Bureau Score is acceptable",
		                                                                   		"Date of birth is correct",
		                                                                   		"Customer is employed",
		                                                                   		"Monthly income limit check",
		                                                                   		"Application data blacklist check",
		                                                                   		"Hardware blacklist check",
		                                                                   		"Credit bureau data is available",
		                                                                   		"Applicant is not minor",
		                                                                   		"Ability to verify personal data",
		                                                                   		"Applicant is alive",
		                                                                   		"Customer is solvent",
																				"Fraud list check"
		                                                                   	};

		#endregion

		[Test, AUT(AUT.Ca, AUT.Za) ]
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

		[Test, AUT(AUT.Ca, AUT.Za)]
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

				case AUT.Ca:
					{
						return ExpectedCheckpointNamesL0Ca;
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
				case AUT.Ca:
					{
						return ExpectedCheckpointNamesLnCa;
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
