using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IMainApplicantAccepted </summary>
    [XmlRoot("IMainApplicantAccepted", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IMainApplicantWorkflowDecision,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IMainApplicantAcceptedEvent : MsmqMessage<IMainApplicantAcceptedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
