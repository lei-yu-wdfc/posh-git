using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Decisions.IMainApplicantWorkflowDeclined </summary>
    [XmlRoot("IMainApplicantWorkflowDeclined", Namespace = "Wonga.Risk.Decisions", DataType = "Wonga.Risk.IWorkflowDeclined,Wonga.Risk.IDeclinedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent,Wonga.Risk.IWorkflowDecision")]
    public partial class IMainApplicantWorkflowDeclinedEvent : MsmqMessage<IMainApplicantWorkflowDeclinedEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
