using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IBasePaymentCardAdded </summary>
    [XmlRoot("IBasePaymentCardAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBasePaymentCardAdded : MsmqMessage<IBasePaymentCardAdded>
    {
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}