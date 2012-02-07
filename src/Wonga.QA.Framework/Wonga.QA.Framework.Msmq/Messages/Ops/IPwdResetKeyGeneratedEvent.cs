using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    [XmlRoot("IPwdResetKeyGenerated", Namespace = "Wonga.Ops.PublicMessages", DataType = "")]
    public partial class IPwdResetKeyGeneratedEvent : MsmqMessage<IPwdResetKeyGeneratedEvent>
    {
        public Guid NotificationId { get; set; }
        public String PwdResetKey { get; set; }
    }
}
