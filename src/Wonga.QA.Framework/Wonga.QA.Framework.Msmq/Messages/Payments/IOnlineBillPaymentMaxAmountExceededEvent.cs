using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.PublicMessages.IOnlineBillPaymentMaxAmountExceeded </summary>
    [XmlRoot("IOnlineBillPaymentMaxAmountExceeded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IOnlineBillPaymentFailed,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IOnlineBillPaymentMaxAmountExceededEvent : MsmqMessage<IOnlineBillPaymentMaxAmountExceededEvent>
    {
        public Guid OnlineBillPaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
