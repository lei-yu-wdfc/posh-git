using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.PublicMessages
{
    /// <summary> Wonga.BankGateway.PublicMessages.IDirectDebitCreated </summary>
    [XmlRoot("IDirectDebitCreated", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.BankGateway, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IDirectDebitCreated : MsmqMessage<IDirectDebitCreated>
    {
        public DateTime CreatedOn { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
