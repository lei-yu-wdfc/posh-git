using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("IBankAccountDeleted", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBankAccountDeletedEvent : MsmqMessage<IBankAccountDeletedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
