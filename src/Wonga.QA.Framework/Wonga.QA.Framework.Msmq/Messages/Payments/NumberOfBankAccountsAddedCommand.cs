using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("NumberOfBankAccountsAdded", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class NumberOfBankAccountsAddedCommand : MsmqMessage<NumberOfBankAccountsAddedCommand>
    {
        public Guid AccountId { get; set; }
        public Int32 NumberOfBankAccountsAddedSinceLastLoan { get; set; }
        public Boolean CanAddFurtherBankAccounts { get; set; }
    }
}
