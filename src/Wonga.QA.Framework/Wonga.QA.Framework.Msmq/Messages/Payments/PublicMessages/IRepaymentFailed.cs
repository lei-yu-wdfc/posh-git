using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentFailed </summary>
    [XmlRoot("IRepaymentFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IRepaymentFailed : MsmqMessage<IRepaymentFailed>
    {
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
