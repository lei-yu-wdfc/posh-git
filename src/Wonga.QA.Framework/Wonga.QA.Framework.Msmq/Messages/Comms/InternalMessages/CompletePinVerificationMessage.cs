using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.CompletePinVerificationMessage </summary>
    [XmlRoot("CompletePinVerificationMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompletePinVerificationMessage : MsmqMessage<CompletePinVerificationMessage>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Pin { get; set; }
    }
}
