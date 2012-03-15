using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Decisions.IGuarantorWorkflowAccepted </summary>
    [XmlRoot("IGuarantorWorkflowAccepted", Namespace = "Wonga.Risk.Decisions", DataType = "Wonga.Risk.IWorkflowAccepted,Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent,Wonga.Risk.IWorkflowDecision")]
    public partial class IGuarantorWorkflowAcceptedEvent : MsmqMessage<IGuarantorWorkflowAcceptedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
