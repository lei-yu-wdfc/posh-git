using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages.Za
{
    /// <summary> Wonga.Payments.PublicMessages.Za.IEasyPayNumberGenerated </summary>
    [XmlRoot("IEasyPayNumberGenerated", Namespace = "Wonga.Payments.PublicMessages.Za", DataType = "Wonga.Payments.PublicMessages.IRepaymentNumberGenerated,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IEasyPayNumberGenerated : MsmqMessage<IEasyPayNumberGenerated>
    {
        public Guid AccountId { get; set; }
        public String RepaymentNumber { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
