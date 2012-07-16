using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.TakeBankPaymentAcknowledged </summary>
    [XmlRoot("TakeBankPaymentAcknowledged", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class TakeBankPaymentAcknowledged : MsmqMessage<TakeBankPaymentAcknowledged>
    {
        public Int32 IntegrationTransactionId { get; set; }
        public String IntegrationName { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
