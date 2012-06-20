using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Email.InternalMessages
{
    /// <summary> Wonga.Email.InternalMessages.SendEmailMessage </summary>
    [XmlRoot("SendEmailMessage", Namespace = "Wonga.Email.InternalMessages", DataType = "")]
    public partial class SendEmailCommand : MsmqMessage<SendEmailCommand>
    {
        public Guid OriginatingSagaId { get; set; }
        public String EmailAddress { get; set; }
        public String HtmlContent { get; set; }
        public String PlainContent { get; set; }
        public String TemplateName { get; set; }
    }
}
