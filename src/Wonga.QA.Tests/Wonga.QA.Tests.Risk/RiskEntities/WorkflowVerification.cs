using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tests.Risk.RiskEntities
{
    public class WorkflowVerification
    {
        public Int32 WorkflowVerificationId { get; set; }
        public Int32? RiskApplicationId { get; set; }
        public Boolean Executed { get; set; }
        public Int32 VerificationDefinitionId { get; set; }
        public Int32? WorkflowCheckpointId { get; set; }

        public List<VerificationDefinition> VerificationDefinitions { get; set; }
        public List<WorkflowCheckpoint> WorkflowCheckpoints { get; set; }
    }
}
