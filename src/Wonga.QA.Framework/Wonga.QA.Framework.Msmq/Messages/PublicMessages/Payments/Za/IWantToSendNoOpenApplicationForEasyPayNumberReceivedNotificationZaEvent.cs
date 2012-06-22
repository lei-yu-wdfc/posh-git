using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotification </summary>
    [XmlRoot("IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotification", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "")]
    public partial class IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotificationZaEvent : MsmqMessage<IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotificationZaEvent>
    {
        public Guid AccountId { get; set; }
        public Int32 AcknowledgeId { get; set; }
    }
}
