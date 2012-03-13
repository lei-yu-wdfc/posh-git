using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IBankAccountAdded </summary>
    [XmlRoot("IBankAccountAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBankAccountAddedEvent : MsmqMessage<IBankAccountAddedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
