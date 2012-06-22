using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SetBankAccountPrimary </summary>
    [XmlRoot("SetBankAccountPrimary", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SetBankAccountPrimaryCommand : MsmqMessage<SetBankAccountPrimaryCommand>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
