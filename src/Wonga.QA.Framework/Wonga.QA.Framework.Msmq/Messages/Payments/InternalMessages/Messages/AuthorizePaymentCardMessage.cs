using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AuthorizePaymentCardMessage </summary>
    [XmlRoot("AuthorizePaymentCardMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AuthorizePaymentCardMessage : MsmqMessage<AuthorizePaymentCardMessage>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
