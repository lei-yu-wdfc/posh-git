using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendSimpleEmailMessage </summary>
    [XmlRoot("SendSimpleEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendSimpleEmailMessage : MsmqMessage<SendSimpleEmailMessage>
    {
        public SimpleEmailEnum SimpleEmailType { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
