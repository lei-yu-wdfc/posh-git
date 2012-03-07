using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.IWorkflowDecision </summary>
    [XmlRoot("IWorkflowDecision", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent")]
    public partial class IWorkflowDecisionPublicEvent : MsmqMessage<IWorkflowDecisionPublicEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
