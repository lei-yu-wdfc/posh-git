using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToGenerateAManualPaymentNotificationResponse </summary>
    [XmlRoot("IWantToGenerateAManualPaymentNotificationResponse", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IWantToGenerateAManualPaymentNotificationResponseEvent : MsmqMessage<IWantToGenerateAManualPaymentNotificationResponseEvent>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid FileId { get; set; }
    }
}
