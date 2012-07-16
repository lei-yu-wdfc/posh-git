using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToSendAManualPaymentNotificationResponse </summary>
    [XmlRoot("IWantToSendAManualPaymentNotificationResponse", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IWantToSendAManualPaymentNotificationResponse : MsmqMessage<IWantToSendAManualPaymentNotificationResponse>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Boolean Successful { get; set; }
    }
}
