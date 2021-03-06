using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateManualPaymentNotificationEmailMessage </summary>
    [XmlRoot("CreateManualPaymentNotificationEmailMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.FileStorage.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateManualPaymentNotificationEmailMessage : MsmqMessage<CreateManualPaymentNotificationEmailMessage>
    {
        public Guid ManualPaymentNotificationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentTransactionScopeEnum PaymentTransactionScope { get; set; }
    }
}
