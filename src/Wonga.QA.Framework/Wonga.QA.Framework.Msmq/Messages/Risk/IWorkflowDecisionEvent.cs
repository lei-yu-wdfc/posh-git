using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowDecision </summary>
    [XmlRoot("IWorkflowDecision", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "")]
    public partial class IWorkflowDecisionEvent : MsmqMessage<IWorkflowDecisionEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
