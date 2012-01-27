using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("AuthorizePaymentCardMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class AuthorizePaymentCardCommand : MsmqMessage<AuthorizePaymentCardCommand>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
