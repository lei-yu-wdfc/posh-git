using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.CompleteMobilePhoneVerificationMessage </summary>
    [XmlRoot("CompleteMobilePhoneVerificationMessage", Namespace = "Wonga.Comms.Commands", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompleteMobilePhoneVerification : MsmqMessage<CompleteMobilePhoneVerification>
    {
        public Guid VerificationId { get; set; }
        public String Pin { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
