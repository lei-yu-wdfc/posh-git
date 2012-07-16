using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.ITopupFundsTransferFailed </summary>
    [XmlRoot("ITopupFundsTransferFailed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ITopupFundsTransferFailed : MsmqMessage<ITopupFundsTransferFailed>
    {
        public Guid AccountId { get; set; }
        public Guid TopupId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
