using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IDoNotRelendStatusChanged", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IDoNotRelendStatusChangedEvent : MsmqMessage<IDoNotRelendStatusChangedEvent>
    {
        public Guid AccountId { get; set; }
        public Boolean DoNotRelend { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
