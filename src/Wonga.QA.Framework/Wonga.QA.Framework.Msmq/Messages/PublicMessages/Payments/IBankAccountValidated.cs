using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments
{
    /// <summary> Wonga.PublicMessages.Payments.IBankAccountValidated </summary>
    [XmlRoot("IBankAccountValidated", Namespace = "Wonga.PublicMessages.Payments", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IBankAccountValidated : MsmqMessage<IBankAccountValidated>
    {
        public Boolean IsValid { get; set; }
        public List<String> Errors { get; set; }
        public Guid BankAccountId { get; set; }
    }
}
