using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IPaymentCardAdded </summary>
    [XmlRoot("IPaymentCardAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IBasePaymentCardAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IPaymentCardAdded : MsmqMessage<IPaymentCardAdded>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}