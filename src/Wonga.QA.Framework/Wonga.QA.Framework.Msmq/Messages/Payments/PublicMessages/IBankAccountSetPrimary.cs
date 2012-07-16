using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IBankAccountSetPrimary </summary>
    [XmlRoot("IBankAccountSetPrimary", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBankAccountSetPrimary : MsmqMessage<IBankAccountSetPrimary>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
