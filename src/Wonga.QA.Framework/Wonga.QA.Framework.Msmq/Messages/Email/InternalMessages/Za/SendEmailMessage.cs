using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages.Za
{
    /// <summary> Wonga.Email.InternalMessages.Za.SendEmailMessage </summary>
    [XmlRoot("SendEmailMessage", Namespace = "Wonga.Email.InternalMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Email.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendEmailMessage : MsmqMessage<SendEmailMessage>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
        public String TemplateName { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
