using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IFundsTransferredInternal </summary>
    [XmlRoot("IFundsTransferredInternal", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IFundsTransferred,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IFundsTransferredInternalEvent : MsmqMessage<IFundsTransferredInternalEvent>
    {
        public Int64 PaymentReference { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
