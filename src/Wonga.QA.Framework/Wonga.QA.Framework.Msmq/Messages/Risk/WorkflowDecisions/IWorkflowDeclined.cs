using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.WorkflowDecisions
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowDeclined </summary>
    [XmlRoot("IWorkflowDeclined", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IWorkflowDeclined : MsmqMessage<IWorkflowDeclined>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
