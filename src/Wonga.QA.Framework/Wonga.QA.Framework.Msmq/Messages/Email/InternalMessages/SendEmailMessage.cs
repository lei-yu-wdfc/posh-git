using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages
{
    /// <summary> Wonga.Email.InternalMessages.SendEmailMessage </summary>
    [XmlRoot("SendEmailMessage", Namespace = "Wonga.Email.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Email.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendEmailMessage : MsmqMessage<SendEmailMessage>
    {
        public Guid OriginatingSagaId { get; set; }
        public String EmailAddress { get; set; }
        public String HtmlContent { get; set; }
        public String PlainContent { get; set; }
        public String TemplateName { get; set; }
    }
}
