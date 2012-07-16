using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.PaymentSentErrorMessage </summary>
    [XmlRoot("PaymentSentErrorMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class PaymentSentErrorMessage : MsmqMessage<PaymentSentErrorMessage>
    {
        public Int64 PaymentReference { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public Guid ApplicationExternalId { get; set; }
        public String ErrorCode { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
