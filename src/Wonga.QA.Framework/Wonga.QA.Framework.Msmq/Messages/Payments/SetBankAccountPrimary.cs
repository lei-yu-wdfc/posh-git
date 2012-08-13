using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SetBankAccountPrimary </summary>
    [XmlRoot("SetBankAccountPrimary", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SetBankAccountPrimary : MsmqMessage<SetBankAccountPrimary>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
