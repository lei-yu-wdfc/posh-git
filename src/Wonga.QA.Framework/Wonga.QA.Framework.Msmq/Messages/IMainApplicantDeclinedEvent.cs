using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.WorkflowDecisions.IMainApplicantDeclined </summary>
    [XmlRoot("IMainApplicantDeclined", Namespace = "Wonga.Risk.WorkflowDecisions", DataType = "Wonga.Risk.WorkflowDecisions.IMainApplicantWorkflowDecision,Wonga.Risk.WorkflowDecisions.IWorkflowDecision")]
    public partial class IMainApplicantDeclinedEvent : MsmqMessage<IMainApplicantDeclinedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
