using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IOnlineBillPaymentMaxAmountExceeded </summary>
    [XmlRoot("IOnlineBillPaymentMaxAmountExceeded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IOnlineBillPaymentFailed,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IOnlineBillPaymentMaxAmountExceeded : MsmqMessage<IOnlineBillPaymentMaxAmountExceeded>
    {
        public Guid OnlineBillPaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
