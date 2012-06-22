using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelRepaymentArrangement </summary>
    [XmlRoot("CancelRepaymentArrangement", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CancelRepaymentArrangementCommand : MsmqMessage<CancelRepaymentArrangementCommand>
    {
        public Guid RepaymentArrangementId { get; set; }
    }
}
