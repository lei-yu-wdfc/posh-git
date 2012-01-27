using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    [XmlRoot("IPasswordChanged", Namespace = "Wonga.Ops.PublicMessages", DataType = "")]
    public class IPasswordChangedEvent : MsmqMessage<IPasswordChangedEvent>
    {
        public Guid AccountId { get; set; }
    }
}
