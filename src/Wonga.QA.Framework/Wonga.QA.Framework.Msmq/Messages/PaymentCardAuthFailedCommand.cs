using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.PaymentCardAuthFailedMessage </summary>
    [XmlRoot("PaymentCardAuthFailedMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class PaymentCardAuthFailedCommand : MsmqMessage<PaymentCardAuthFailedCommand>
    {
        public CardPaymentStatusCodeEnum FailureCode { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
