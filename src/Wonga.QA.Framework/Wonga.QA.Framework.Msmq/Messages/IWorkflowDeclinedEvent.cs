using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowDeclined </summary>
    [XmlRoot("IWorkflowDeclined", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IWorkflowDeclinedEvent : MsmqMessage<IWorkflowDeclinedEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
