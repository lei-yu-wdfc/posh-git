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
		private readonly List<string> ExpectedCheckpointNames = GetExpectedCheckpointNames();

		private static List<string> _expectedCheckpointNamesZa = new List<string>();
 		
		private readonly List<string> ExpectedVerificationNames = GetExpectedVerificationNames();

		private static List<string> _expectedVerificationNamesZa = new List<string>();

		[Test, AUT(AUT.Za), Explicit]
		public void WorkflowL0SingleWorkflowUsed()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			//var workflows = Drive.Db.GetWorkflowsForApplication(application.Id);

			//Assert.AreEqual(1, workflows.Count);
		}

		[Test, AUT(AUT.Za), Explicit]
		public void WorkflowCorrectCheckpointsUsedL0()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
                .WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			//var workflow = Drive.Db.GetWorkflowsForApplication(application.Id).FirstOrDefault();

			//Assert.AreElementsEqualIgnoringOrder(ExpectedCheckpointNames, actualCheckpointNames);
		}

		[Test, AUT(AUT.Za), Explicit]
		public void WorkflowLnCorrectCheckpointsUsed()
		{
			
		}

		#region Helpers

		private static List<string> GetExpectedCheckpointNames()
		{
			switch (Config.AUT)
			{
					case AUT.Za:
					{
						return _expectedCheckpointNamesZa;
					}
					break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		private static List<string> GetExpectedVerificationNames()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						return _expectedVerificationNamesZa;
					}
					break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		private IEnumerable<string> GetApplicationCheckpointNames(Application application)
		{
			var db = new DbDriver();

			return (from rw in db.Risk.RiskWorkflows
					where rw.ApplicationId == application.Id
						join wc in db.Risk.WorkflowCheckpoints
						on rw.RiskWorkflowId equals wc.RiskWorkflowId
							join cd in db.Risk.CheckpointDefinitions
							on wc.CheckpointDefinitionId equals cd.CheckpointDefinitionId
			        select cd.Name);
		}

		private IQueryable<string> GetApplicationVerificationNames(Application application)
		{
			var db = new DbDriver();

			return (from rw in db.Risk.RiskWorkflows
					where rw.ApplicationId == application.Id
					join wv in db.Risk.WorkflowVerifications
					on rw.RiskWorkflowId equals wv.RiskWorkflowId
					join vd in db.Risk.VerificationDefinitions
					on wv.VerificationDefinitionId equals vd.VerificationDefinitionId
					select vd.Name);
		}
		#endregion
	}
}
