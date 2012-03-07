using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.IPwdResetNotificationSent </summary>
    [XmlRoot("IPwdResetNotificationSent", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IPwdResetNotificationSentEvent : MsmqMessage<IPwdResetNotificationSentEvent>
    {
        public Guid NotificationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
