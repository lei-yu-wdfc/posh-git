using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IOnlineBillPaymentCcinMismatched </summary>
    [XmlRoot("IOnlineBillPaymentCcinMismatched", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IOnlineBillPaymentFailed,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IOnlineBillPaymentCcinMismatchedEvent : MsmqMessage<IOnlineBillPaymentCcinMismatchedEvent>
    {
        public Guid OnlineBillPaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
