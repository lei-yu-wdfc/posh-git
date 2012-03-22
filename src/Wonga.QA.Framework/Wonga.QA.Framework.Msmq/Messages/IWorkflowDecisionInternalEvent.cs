using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IWorkflowDecision </summary>
    [XmlRoot("IWorkflowDecision", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "")]
    public partial class IWorkflowDecisionInternalEvent : MsmqMessage<IWorkflowDecisionInternalEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
