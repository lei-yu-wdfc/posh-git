using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IBankAccountActivated </summary>
    [XmlRoot("IBankAccountActivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBankAccountActivated : MsmqMessage<IBankAccountActivated>
    {
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
