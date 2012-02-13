using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tests.Risk.RiskEntities
{
    public class WorkflowCheckpoint
    {
        public Int32 CheckpointStatus { get; set; }
        public List<CheckpointDefinition> CheckpointDefinitions { get; set; }
    }
}
