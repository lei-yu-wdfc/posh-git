using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Decisions.IMainApplicantWorkflowAccepted </summary>
    [XmlRoot("IMainApplicantWorkflowAccepted", Namespace = "Wonga.Risk.Decisions", DataType = "Wonga.Risk.IWorkflowAccepted,Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent,Wonga.Risk.IWorkflowDecision")]
    public partial class IMainApplicantWorkflowAcceptedEvent : MsmqMessage<IMainApplicantWorkflowAcceptedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
