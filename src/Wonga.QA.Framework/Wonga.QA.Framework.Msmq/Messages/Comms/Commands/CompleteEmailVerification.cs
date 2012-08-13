using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.CompleteEmailVerificationMessage </summary>
    [XmlRoot("CompleteEmailVerificationMessage", Namespace = "Wonga.Comms.Commands", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompleteEmailVerification : MsmqMessage<CompleteEmailVerification>
    {
        public Guid AccountId { get; set; }
        public Guid ChangeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
