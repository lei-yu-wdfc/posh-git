using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IPaymentCardDeactivated </summary>
    [XmlRoot("IPaymentCardDeactivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IPaymentCardDeactivated : MsmqMessage<IPaymentCardDeactivated>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}