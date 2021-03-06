using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.StartEmailVerificationMessage </summary>
    [XmlRoot("StartEmailVerificationMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StartEmailVerificationMessage : MsmqMessage<StartEmailVerificationMessage>
    {
        public Guid AccountId { get; set; }
        public String Forename { get; set; }
        public String Email { get; set; }
        public Guid ChangeId { get; set; }
        public String UriFragment { get; set; }
        public Int32 TimeoutMinutes { get; set; }
    }
}
