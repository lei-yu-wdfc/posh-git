using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IDisputeStatusChanged", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IDisputeStatusChangedEvent : MsmqMessage<IDisputeStatusChangedEvent>
    {
        public Guid AccountId { get; set; }
        public Boolean HasDispute { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
