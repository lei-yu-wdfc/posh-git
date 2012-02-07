using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ITransactionProcessed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class ITransactionProcessedEvent : MsmqMessage<ITransactionProcessedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
