using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelRepaymentArrangement </summary>
    [XmlRoot("CancelRepaymentArrangement", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CancelRepaymentArrangement : MsmqMessage<CancelRepaymentArrangement>
    {
        public Guid RepaymentArrangementId { get; set; }
    }
}
