using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AuthorizePaymentCardMessage </summary>
    [XmlRoot("AuthorizePaymentCardMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AuthorizePaymentCardCommand : MsmqMessage<AuthorizePaymentCardCommand>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
