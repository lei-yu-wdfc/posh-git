using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.ITopupFundsTransferred </summary>
    [XmlRoot("ITopupFundsTransferred", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ITopupFundsTransferredEvent : MsmqMessage<ITopupFundsTransferredEvent>
    {
        public Guid AccountId { get; set; }
        public Guid TopupId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
