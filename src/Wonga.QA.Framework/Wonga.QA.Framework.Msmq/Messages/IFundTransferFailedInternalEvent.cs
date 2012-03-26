using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IFundTransferFailedInternal </summary>
    [XmlRoot("IFundTransferFailedInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IFundTransferFailed,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IFundTransferFailedInternalEvent : MsmqMessage<IFundTransferFailedInternalEvent>
    {
        public Int64 PaymentReference { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
