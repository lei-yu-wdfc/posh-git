using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IApplicationAccepted", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IAcceptedDecision,Wonga.Risk.IApplicationDecision,Wonga.Risk.IRiskEvent")]
    public partial class IApplicationAcceptedEvent : MsmqMessage<IApplicationAcceptedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
