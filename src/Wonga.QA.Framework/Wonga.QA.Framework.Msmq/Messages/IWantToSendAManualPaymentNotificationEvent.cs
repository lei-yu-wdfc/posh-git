using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToSendAManualPaymentNotification </summary>
    [XmlRoot("IWantToSendAManualPaymentNotification", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IWantToSendAManualPaymentNotificationEvent : MsmqMessage<IWantToSendAManualPaymentNotificationEvent>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid FileId { get; set; }
    }
}
