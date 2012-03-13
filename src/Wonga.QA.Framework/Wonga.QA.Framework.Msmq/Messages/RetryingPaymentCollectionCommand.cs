using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.RetryingPaymentCollectionMessage </summary>
    [XmlRoot("RetryingPaymentCollectionMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class RetryingPaymentCollectionCommand : MsmqMessage<RetryingPaymentCollectionCommand>
    {
        public Int32 ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
