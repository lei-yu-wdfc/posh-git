using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentArrangementCancelled </summary>
    [XmlRoot("IRepaymentArrangementCancelled", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IRepaymentArrangementCancelled : MsmqMessage<IRepaymentArrangementCancelled>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
