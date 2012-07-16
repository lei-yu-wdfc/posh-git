using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IOnlineBillPaymentFailed </summary>
    [XmlRoot("IOnlineBillPaymentFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IOnlineBillPaymentFailed : MsmqMessage<IOnlineBillPaymentFailed>
    {
        public Guid OnlineBillPaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
