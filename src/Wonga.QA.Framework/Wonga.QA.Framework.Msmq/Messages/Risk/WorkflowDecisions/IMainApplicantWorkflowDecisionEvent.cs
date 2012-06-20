using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.WorkflowDecisions
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IMainApplicantWorkflowDecision </summary>
    [XmlRoot("IMainApplicantWorkflowDecision", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IMainApplicantWorkflowDecisionEvent : MsmqMessage<IMainApplicantWorkflowDecisionEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
