using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("DeleteBankAccount", Namespace = "Wonga.Payments", DataType = "")]
    public class DeleteBankAccountCommand : MsmqMessage<DeleteBankAccountCommand>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
