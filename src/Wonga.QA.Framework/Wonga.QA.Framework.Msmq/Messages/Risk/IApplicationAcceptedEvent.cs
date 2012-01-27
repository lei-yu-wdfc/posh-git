using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IApplicationAccepted", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IApplicationDecision,Wonga.Risk.IAcceptedDecision,Wonga.Risk.IRiskEvent")]
    public class IApplicationAcceptedEvent : MsmqMessage<IApplicationAcceptedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
