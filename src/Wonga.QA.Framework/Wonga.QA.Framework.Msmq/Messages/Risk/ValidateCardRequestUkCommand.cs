using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("ValidateCardRequestMessage", Namespace = "Wonga.Risk.CardPayment", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ValidateCardRequestUkCommand : MsmqMessage<ValidateCardRequestUkCommand>
    {
        public Guid PaymentCardID { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
