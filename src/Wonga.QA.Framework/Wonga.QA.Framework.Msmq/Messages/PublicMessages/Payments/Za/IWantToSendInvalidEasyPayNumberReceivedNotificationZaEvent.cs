using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToSendInvalidEasyPayNumberReceivedNotification </summary>
    [XmlRoot("IWantToSendInvalidEasyPayNumberReceivedNotification", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "")]
    public partial class IWantToSendInvalidEasyPayNumberReceivedNotificationZaEvent : MsmqMessage<IWantToSendInvalidEasyPayNumberReceivedNotificationZaEvent>
    {
        public Int32 AcknowledgeId { get; set; }
    }
}
