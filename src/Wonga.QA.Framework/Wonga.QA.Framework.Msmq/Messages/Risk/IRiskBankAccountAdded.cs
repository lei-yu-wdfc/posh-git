using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskBankAccountAdded </summary>
    [XmlRoot("IRiskBankAccountAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IRiskBankAccountAdded : MsmqMessage<IRiskBankAccountAdded>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
