using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.HyphenAccountVerificationMessage </summary>
    [XmlRoot("HyphenAccountVerificationMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Hyphen.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HyphenAccountVerificationMessage : MsmqMessage<HyphenAccountVerificationMessage>
    {
        public Int32 BankAccountVerificationId { get; set; }
    }
}
