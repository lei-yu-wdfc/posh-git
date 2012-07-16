using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.PaymentCardAuthorizedMessage </summary>
    [XmlRoot("PaymentCardAuthorizedMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class PaymentCardAuthorizedMessage : MsmqMessage<PaymentCardAuthorizedMessage>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
