using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToGenerateAManualPaymentNotification </summary>
    [XmlRoot("IWantToGenerateAManualPaymentNotification", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IWantToGenerateAManualPaymentNotificationEvent : MsmqMessage<IWantToGenerateAManualPaymentNotificationEvent>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentTransactionScopeEnum PaymentTransactionScope { get; set; }
    }
}
