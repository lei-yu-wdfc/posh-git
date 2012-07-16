using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.DeleteBankAccount </summary>
    [XmlRoot("DeleteBankAccount", Namespace = "Wonga.Payments", DataType = "")]
    public partial class DeleteBankAccount : MsmqMessage<DeleteBankAccount>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
