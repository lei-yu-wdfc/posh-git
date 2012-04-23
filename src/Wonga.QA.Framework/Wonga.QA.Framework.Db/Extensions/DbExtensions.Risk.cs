using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework.Db.Extensions
{
	public static partial class DbExtensions
	{
		public static void UpdateEmployerName(this DbDriver db, Guid accountId, string employerName)
		{
			db.Risk.EmploymentDetails.Single(a => a.AccountId == accountId).EmployerName = employerName;
			db.Risk.SubmitChanges();
		}

        public static void UpdateMiddleName(this DbDriver db, Guid accountId, string middleName)
        {
            db.Risk.RiskAccounts.Single(a => a.AccountId == accountId).Middlename = middleName;
            db.Risk.SubmitChanges();
        }

		public static void RemovePhoneNumberFromRiskDb(this DbDriver db, String mobilePhoneNumber)
		{
			//Clean the mobile number from DB 
			var riskDb = db.Risk;
			var entities = riskDb.RiskAccountMobilePhones.Where(p => p.MobilePhone == mobilePhoneNumber).ToList();
			if (entities.Count <= 0) return;
			riskDb.RiskAccountMobilePhones.DeleteAllOnSubmit(entities);
			riskDb.SubmitChanges();
		}

		public static void AddPhoneNumberToRiskDb(this DbDriver db, String mobilePhoneNumber)
		{
			var tempId = Guid.NewGuid();
			var riskDb = db.Risk;

			//Add the mobile number to Risk DB 
			var riskAccountEntity = new RiskAccountEntity()
			                        	{
			                        		AccountId = tempId,
			                        		AccountRank = 1,
			                        		HasAccount = true,
			                        		CreditLimit = 400,
			                        		ConfirmedFraud = false,
			                        		DateOfBirth = Get.GetDoB(),
			                        		DoNotRelend = false,
			                        		Forename = Get.RandomString(8),
			                        		IsDebtSale = false,
			                        		IsDispute = false,
			                        		IsHardship = false,
			                        		Postcode = "KT2 5DL",
			                        		MaidenName = Get.RandomString(8),
			                        		Middlename = Get.RandomString(8),
			                        		Surname = Get.RandomString(8)
			                        	};
			var riskAccoutnMobilePhoneEntity = new RiskAccountMobilePhoneEntity()
			                                   	{
			                                   		AccountId = tempId,
			                                   		DateUpdated = new DateTime(2010, 10, 06).ToDate(),
			                                   		MobilePhone = mobilePhoneNumber,
			                                   	};

			riskDb.RiskAccounts.InsertOnSubmit(riskAccountEntity);
			riskDb.RiskAccountMobilePhones.InsertOnSubmit(riskAccoutnMobilePhoneEntity);
			riskDb.SubmitChanges();
		}

		public static List<RiskWorkflowEntity> GetWorkflowsForApplication(this DbDriver db, Guid applicationId)
		{
			return db.Risk.RiskWorkflows.Where(p => p.ApplicationId == applicationId).ToList();
		}

		public static List<RiskWorkflowEntity> GetWorkflowsForApplication(this DbDriver db, Guid applicationId, RiskWorkflowTypes workflowType)
		{
			return db.Risk.RiskWorkflows.Where(p => p.ApplicationId == applicationId && (RiskWorkflowTypes)p.WorkflowType == workflowType).ToList();
		}

		public static IEnumerable<CheckpointDefinitionEntity> GetCheckpointDefinitionsForApplication(this DbDriver db, Guid applicationId)
		{
			var workflows = GetWorkflowsForApplication(db, applicationId);

			var checkpoints = new List<CheckpointDefinitionEntity>();

			foreach (var workflow in workflows)
			{
				checkpoints.AddRange(GetCheckpointDefinitionsForWorkflow(db, workflow.RiskWorkflowId));
			}

			return checkpoints;
		}

		public static IEnumerable<CheckpointDefinitionEntity> GetCheckpointDefinitionsForWorkflow(this DbDriver db, int workflowId)
		{
			var checkpointIds = (from rw in db.Risk.RiskWorkflows
						  join wc in db.Risk.WorkflowCheckpoints on rw.RiskWorkflowId equals wc.RiskWorkflowId
						  where rw.RiskWorkflowId == workflowId
			              select wc.CheckpointDefinitionEntity);

			var result = (from c in checkpointIds
			              join cd in db.Risk.CheckpointDefinitions on c.CheckpointDefinitionId equals cd.CheckpointDefinitionId
			              select cd);
			
			return result;
		}

		public static IEnumerable<VerificationDefinitionEntity> GetVerificationDefinitionsForApplication(this DbDriver db, Guid applicationId)
		{
			var workflows = GetWorkflowsForApplication(db, applicationId);

			var verifications = new List<VerificationDefinitionEntity>();

			foreach (var workflow in workflows)
			{
				verifications.AddRange(GetVerificationDefinitionsForWorkflow(db, workflow.RiskWorkflowId));
			}

			return verifications;
		}

		public static IEnumerable<VerificationDefinitionEntity> GetVerificationDefinitionsForWorkflow(this DbDriver db, int workflowId)
		{
			var verifcationDefinitionIds = (from rw in db.Risk.RiskWorkflows
			                                where rw.RiskWorkflowId == workflowId
			                                join wv in db.Risk.WorkflowVerifications on rw.RiskWorkflowId equals
			                                	wv.RiskWorkflowId
												orderby wv.SortOrder
			                                select wv.VerificationDefinitionId);

			var result = (from v in verifcationDefinitionIds
			              join vd in db.Risk.VerificationDefinitions on v equals vd.VerificationDefinitionId
			              select vd);

			return result;
		}

		public static List<String> GetExecutedCheckpointsDefinitionNamesForApplicationId(this DbDriver db, Guid applicationId, params RiskCheckpointStatus[] expectedStatus)
		{
			var riskWorkflowEntity = db.Risk.RiskWorkflows.SingleOrDefault(r => r.ApplicationId == applicationId);
			var executedCheckpoints = new List<string>();

			if (riskWorkflowEntity != null)
			{
				var executedCheckpointIds = expectedStatus.Any()
				                            	? db.Risk.WorkflowCheckpoints.Where(
				                            		p =>
				                            		p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId &&
				                            		expectedStatus.Contains((RiskCheckpointStatus)p.CheckpointStatus)).Select(
				                            			p => p.CheckpointDefinitionId).ToList()
				                            	: db.Risk.WorkflowCheckpoints.Where(
				                            		p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId).
				                            	  	Select(p => p.CheckpointDefinitionId).ToList();
				executedCheckpoints.AddRange(db.Risk.CheckpointDefinitions.Where(p => executedCheckpointIds.Contains(p.CheckpointDefinitionId)).Select(p => p.Name));
				return executedCheckpoints;
			}
			return executedCheckpoints;
		}

		public static List<String> GetExecutedCheckpointDefinitionNamesForRiskWorkflow(this DbDriver db, Guid workflowId, params RiskCheckpointStatus[] expectedStatus)
		{
			var riskWorkflowEntity = db.Risk.RiskWorkflows.SingleOrDefault(r => r.WorkflowId == workflowId);
			var executedCheckpoints = new List<string>();

			if (riskWorkflowEntity != null)
			{
				var executedCheckpointIds = expectedStatus.Any()
				                            	? db.Risk.WorkflowCheckpoints.Where(
				                            		p =>
				                            		p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId &&
				                            		expectedStatus.Contains((RiskCheckpointStatus)p.CheckpointStatus)).Select(
				                            			p => p.CheckpointDefinitionId).ToList()
				                            	: db.Risk.WorkflowCheckpoints.Where(
				                            		p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId).
				                            	  	Select(p => p.CheckpointDefinitionId).ToList();
				executedCheckpoints.AddRange(db.Risk.CheckpointDefinitions.Where(p => executedCheckpointIds.Contains(p.CheckpointDefinitionId)).Select(p => p.Name));
				return executedCheckpoints;
			}
			return executedCheckpoints;
		}

		public static List<String> GetExecutedVerificationDefinitionNamesForRiskWorkflow(this DbDriver db, Guid workflowId)
		{
			var riskWorkflowEntity = db.Risk.RiskWorkflows.SingleOrDefault(r => r.WorkflowId == workflowId);
			var executedVerifications = new List<string>();

			if (riskWorkflowEntity != null)
			{
				var executedVerificationsIds =
					db.Risk.WorkflowVerifications.Where(p => p.RiskWorkflowId == riskWorkflowEntity.RiskWorkflowId).
						Select(p => p.VerificationDefinitionId).ToList();

				executedVerifications.AddRange(db.Risk.VerificationDefinitions.Where(p => executedVerificationsIds.Contains(p.VerificationDefinitionId)).Select(p => p.Name));
				return executedVerifications;
			}
			return executedVerifications;
		}

        public static void WaitForRiskWorkflowData(this DbDriver db, Guid applicationId, RiskWorkflowTypes riskWorkflowType,Int32 expectedNumberOfWorkflows,RiskWorkflowStatus workflowStatus)
        {
            Do.Until(
                () =>
                db.Risk.RiskWorkflows.Count(
                    p => p.ApplicationId == applicationId && (RiskWorkflowStatus) p.Decision == workflowStatus && (RiskWorkflowTypes)p.WorkflowType == riskWorkflowType) == expectedNumberOfWorkflows);
        }

        public static void WaitForWorkflowCheckpointData(this DbDriver db ,int riskWorkflowId)
        {
            Do.Until(() => db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == riskWorkflowId && p.CheckpointStatus != 0));
        }
	}
}
