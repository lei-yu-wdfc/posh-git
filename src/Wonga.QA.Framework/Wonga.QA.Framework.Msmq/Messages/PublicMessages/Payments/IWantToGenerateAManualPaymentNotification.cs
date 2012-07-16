using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments
{
    /// <summary> Wonga.PublicMessages.Payments.IWantToGenerateAManualPaymentNotification </summary>
    [XmlRoot("IWantToGenerateAManualPaymentNotification", Namespace = "Wonga.PublicMessages.Payments", DataType = "")]
    public partial class IWantToGenerateAManualPaymentNotification : MsmqMessage<IWantToGenerateAManualPaymentNotification>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentTransactionScopeEnum PaymentTransactionScope { get; set; }
    }
}
