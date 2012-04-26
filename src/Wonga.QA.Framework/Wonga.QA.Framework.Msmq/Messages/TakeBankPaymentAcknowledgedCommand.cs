using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.TakeBankPaymentAcknowledged </summary>
    [XmlRoot("TakeBankPaymentAcknowledged", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class TakeBankPaymentAcknowledgedCommand : MsmqMessage<TakeBankPaymentAcknowledgedCommand>
    {
        public Int32 IntegrationTransactionId { get; set; }
        public String IntegrationName { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
