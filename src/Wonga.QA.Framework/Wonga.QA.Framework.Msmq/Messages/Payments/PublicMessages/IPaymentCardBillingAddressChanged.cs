using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IPaymentCardBillingAddressChanged </summary>
    [XmlRoot("IPaymentCardBillingAddressChanged", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IPaymentCardBillingAddressChanged : MsmqMessage<IPaymentCardBillingAddressChanged>
    {
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
