using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.PublicMessages.IOnlineBillPaymentFailed </summary>
    [XmlRoot("IOnlineBillPaymentFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IOnlineBillPaymentFailedEvent : MsmqMessage<IOnlineBillPaymentFailedEvent>
    {
        public Guid OnlineBillPaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
