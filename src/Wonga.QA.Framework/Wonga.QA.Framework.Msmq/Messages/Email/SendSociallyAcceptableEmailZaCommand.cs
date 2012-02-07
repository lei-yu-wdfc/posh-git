using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Email
{
    [XmlRoot("SendSociallyAcceptableEmailMessage", Namespace = "Wonga.Email.InternalMessages.Za", DataType = "")]
    public partial class SendSociallyAcceptableEmailZaCommand : MsmqMessage<SendSociallyAcceptableEmailZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
        public String TemplateName { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
