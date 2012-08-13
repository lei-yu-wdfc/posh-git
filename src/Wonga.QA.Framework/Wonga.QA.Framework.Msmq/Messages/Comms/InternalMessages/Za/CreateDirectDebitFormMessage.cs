using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Za.CreateDirectDebitFormMessage </summary>
    [XmlRoot("CreateDirectDebitFormMessage", Namespace = "Wonga.Comms.InternalMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateDirectDebitFormMessage : MsmqMessage<CreateDirectDebitFormMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
