using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
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
	}
}
