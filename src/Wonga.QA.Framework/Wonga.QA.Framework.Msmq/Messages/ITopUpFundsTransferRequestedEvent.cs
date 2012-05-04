using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.ITopUpFundsTransferRequested </summary>
    [XmlRoot("ITopUpFundsTransferRequested", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ITopUpFundsTransferRequestedEvent : MsmqMessage<ITopUpFundsTransferRequestedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopUpId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
