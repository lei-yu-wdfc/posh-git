using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IApplicationDeclined", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IApplicationDecision,Wonga.Risk.IDeclinedDecision,Wonga.Risk.IRiskEvent")]
    public class IApplicationDeclinedEvent : MsmqMessage<IApplicationDeclinedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public String FailedCheckpointName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
