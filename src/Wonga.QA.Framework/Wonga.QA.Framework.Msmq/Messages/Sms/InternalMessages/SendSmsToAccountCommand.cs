using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages
{
    /// <summary> Wonga.Sms.InternalMessages.SendSmsToAccountMessage </summary>
    [XmlRoot("SendSmsToAccountMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "")]
    public partial class SendSmsToAccountCommand : MsmqMessage<SendSmsToAccountCommand>
    {
        public Guid SmsMessageId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
    }
}
