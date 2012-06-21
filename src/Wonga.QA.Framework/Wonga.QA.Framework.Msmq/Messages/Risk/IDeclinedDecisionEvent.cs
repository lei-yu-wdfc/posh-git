using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IDeclinedDecision </summary>
    [XmlRoot("IDeclinedDecision", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent")]
    public partial class IDeclinedDecisionEvent : MsmqMessage<IDeclinedDecisionEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
