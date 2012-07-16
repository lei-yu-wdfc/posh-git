using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RequestPartPayment </summary>
    [XmlRoot("RequestPartPayment", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class RequestPartPayment : MsmqMessage<RequestPartPayment>
    {
        public Guid SagaId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal PartPaymentAmount { get; set; }
        public Object PaymentCardCv2 { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
