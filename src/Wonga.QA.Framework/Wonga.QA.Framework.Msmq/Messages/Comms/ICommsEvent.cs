using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ICommsEvent", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ICommsEvent : MsmqMessage<ICommsEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}
