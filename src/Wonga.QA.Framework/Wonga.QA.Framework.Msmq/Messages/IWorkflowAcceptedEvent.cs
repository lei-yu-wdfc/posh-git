using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowAccepted </summary>
    [XmlRoot("IWorkflowAccepted", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IWorkflowAcceptedEvent : MsmqMessage<IWorkflowAcceptedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
