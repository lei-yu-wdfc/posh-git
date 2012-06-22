using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IBankAccountDeactivated </summary>
    [XmlRoot("IBankAccountDeactivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBankAccountDeactivatedEvent : MsmqMessage<IBankAccountDeactivatedEvent>
    {
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
