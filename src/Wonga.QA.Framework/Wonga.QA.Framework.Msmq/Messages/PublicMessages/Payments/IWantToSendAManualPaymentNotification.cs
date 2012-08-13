using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToSendAManualPaymentNotification </summary>
    [XmlRoot("IWantToSendAManualPaymentNotification", Namespace = "Wonga.PublicMessages.Payments", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendAManualPaymentNotification : MsmqMessage<IWantToSendAManualPaymentNotification>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid FileId { get; set; }
    }
}
