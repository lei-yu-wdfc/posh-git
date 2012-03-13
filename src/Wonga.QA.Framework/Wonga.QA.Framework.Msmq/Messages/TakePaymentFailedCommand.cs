using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.TakePaymentFailedMessage </summary>
    [XmlRoot("TakePaymentFailedMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class TakePaymentFailedCommand : MsmqMessage<TakePaymentFailedCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public CardPaymentStatusCodeEnum FailureCode { get; set; }
        public Int64 PaymentReference { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
