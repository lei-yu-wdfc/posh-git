using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.NumberOfBankAccountsAdded </summary>
    [XmlRoot("NumberOfBankAccountsAdded", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class NumberOfBankAccountsAdded : MsmqMessage<NumberOfBankAccountsAdded>
    {
        public Guid AccountId { get; set; }
        public Int32 NumberOfBankAccountsAddedSinceLastLoan { get; set; }
        public Boolean CanAddFurtherBankAccounts { get; set; }
    }
}
