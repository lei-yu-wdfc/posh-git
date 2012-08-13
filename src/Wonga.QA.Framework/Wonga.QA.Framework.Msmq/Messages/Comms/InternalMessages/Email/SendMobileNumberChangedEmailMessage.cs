using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendMobileNumberChangedEmailMessage </summary>
    [XmlRoot("SendMobileNumberChangedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendMobileNumberChangedEmailMessage : MsmqMessage<SendMobileNumberChangedEmailMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
