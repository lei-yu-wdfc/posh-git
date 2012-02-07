using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Common
{
    [XmlRoot("IEventMessage", Namespace = "Wonga.Api", DataType = "")]
    public partial class IEvent : MsmqMessage<IEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}
