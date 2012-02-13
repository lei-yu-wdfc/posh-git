using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Risk.RiskEntities;

namespace Wonga.QA.Tests.Risk
{
    internal class RiskFunctions
    {
        internal List<WorkflowVerification> GetWorkflowVerificationsForRiskApplicationId(Int32 riskApplicationId)
        {
            return (Driver.Db.Risk.WorkflowVerifications.Where(wv => wv.RiskApplicationId == riskApplicationId).Select(
                wv => new WorkflowVerification
                {
                    Executed = wv.Executed,
                    RiskApplicationId = wv.RiskApplicationId,
                    WorkflowVerificationId = wv.WorkflowVerificationId,
                    VerificationDefinitionId = wv.VerificationDefinitionId,
                    WorkflowCheckpointId = wv.WorkflowCheckpointId,
                    VerificationDefinitions = ((Driver.Db.Risk.VerificationDefinitions.Where(
                        vd => vd.VerificationDefinitionId == wv.VerificationDefinitionId).Select(
                            vd => new VerificationDefinition
                            {
                                Name = vd.Name,
                                VerificationDefinitionId = vd.VerificationDefinitionId
                            })).ToList()),
                    WorkflowCheckpoints = ((Driver.Db.Risk.WorkflowCheckpoints.Where(
                        wc => wc.WorkflowCheckpointId == wv.WorkflowCheckpointId).Select(
                            wc => new WorkflowCheckpoint
                            {
                                CheckpointStatus = wc.CheckpointStatus,
                                CheckpointDefinitions =
                                    ((Driver.Db.Risk.CheckpointDefinitions.Where(
                                        cd => cd.CheckpointDefinitionId == wc.CheckpointDefinitionId).Select(
                                            cd => new CheckpointDefinition
                                            {
                                                CheckpointDefinitionId = cd.CheckpointDefinitionId,
                                                Name = cd.Name,
                                                TypeName = cd.TypeName
                                            })).ToList())
                            })).ToList())
                })).ToList();
        }
        internal IEnumerable<int> GetVerificationIdsByTypeNames(string[] verificationTypeNames)
        {
            return verificationTypeNames.Select(v => Driver.Db.Risk.VerificationDefinitions.Single(a => a.TypeName == v).VerificationDefinitionId);
        }
        internal IQueryable<int> GetApplicationVerificationsIds(int riskApplicationId)
        {
            return (Driver.Db.Risk.WorkflowVerifications.Where(r => r.RiskApplicationId == riskApplicationId).Select(r => r.VerificationDefinitionId));
        }
        internal int GetCheckPointStatus(int workflowCheckpointId)
        {
            return (Driver.Db.Risk.WorkflowCheckpoints.Where(p => p.WorkflowCheckpointId == workflowCheckpointId).Select(p => p.CheckpointStatus)).SingleOrDefault();
        }
    }
}
