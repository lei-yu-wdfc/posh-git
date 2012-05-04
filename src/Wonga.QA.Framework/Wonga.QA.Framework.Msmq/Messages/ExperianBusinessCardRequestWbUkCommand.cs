using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Experian.ExperianBusinessCardRequestMessage </summary>
    [XmlRoot("ExperianBusinessCardRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianBusinessCardRequestWbUkCommand : MsmqMessage<ExperianBusinessCardRequestWbUkCommand>
    {
        public Guid PaymentCardId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
