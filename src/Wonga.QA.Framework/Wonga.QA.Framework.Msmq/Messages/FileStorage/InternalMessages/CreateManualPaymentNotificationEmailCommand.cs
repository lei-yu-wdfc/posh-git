using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.FileStorage.InternalMessages;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateManualPaymentNotificationEmailMessage </summary>
    [XmlRoot("CreateManualPaymentNotificationEmailMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateManualPaymentNotificationEmailCommand : MsmqMessage<CreateManualPaymentNotificationEmailCommand>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentTransactionScopeEnum PaymentTransactionScope { get; set; }
    }
}
