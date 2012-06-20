using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IPwdResetNotificationSent </summary>
    [XmlRoot("IPwdResetNotificationSent", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IPwdResetNotificationSentEvent : MsmqMessage<IPwdResetNotificationSentEvent>
    {
        public Guid NotificationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
