using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IRepaymentNumberGenerated </summary>
    [XmlRoot("IRepaymentNumberGenerated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IRepaymentNumberGenerated : MsmqMessage<IRepaymentNumberGenerated>
    {
        public Guid AccountId { get; set; }
        public String RepaymentNumber { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
