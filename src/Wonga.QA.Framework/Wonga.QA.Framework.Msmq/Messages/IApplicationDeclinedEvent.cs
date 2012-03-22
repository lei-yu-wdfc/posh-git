using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.IApplicationDeclined </summary>
    [XmlRoot("IApplicationDeclined", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IDeclinedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent")]
    public partial class IApplicationDeclinedEvent : MsmqMessage<IApplicationDeclinedEvent>
    {
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
