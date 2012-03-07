using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AuthorizeBusinessPaymentCardMessage </summary>
    [XmlRoot("AuthorizeBusinessPaymentCardMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AuthorizeBusinessPaymentCardCommand : MsmqMessage<AuthorizeBusinessPaymentCardCommand>
    {
        public Guid SagaId { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
