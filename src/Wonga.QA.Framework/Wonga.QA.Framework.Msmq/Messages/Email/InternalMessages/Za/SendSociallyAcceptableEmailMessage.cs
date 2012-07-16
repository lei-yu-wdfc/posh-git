using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages.Za
{
    /// <summary> Wonga.Email.InternalMessages.Za.SendSociallyAcceptableEmailMessage </summary>
    [XmlRoot("SendSociallyAcceptableEmailMessage", Namespace = "Wonga.Email.InternalMessages.Za", DataType = "")]
    public partial class SendSociallyAcceptableEmailMessage : MsmqMessage<SendSociallyAcceptableEmailMessage>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
        public String TemplateName { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
