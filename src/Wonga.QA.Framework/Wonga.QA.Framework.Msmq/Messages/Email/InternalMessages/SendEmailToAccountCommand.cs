using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages
{
    /// <summary> Wonga.Email.InternalMessages.SendEmailToAccountMessage </summary>
    [XmlRoot("SendEmailToAccountMessage", Namespace = "Wonga.Email.InternalMessages", DataType = "")]
    public partial class SendEmailToAccountCommand : MsmqMessage<SendEmailToAccountCommand>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
        public String TemplateName { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
