using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IGuarantorWorkflowDeclined </summary>
    [XmlRoot("IGuarantorWorkflowDeclined", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDeclined,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IGuarantorWorkflowDeclinedEvent : MsmqMessage<IGuarantorWorkflowDeclinedEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
