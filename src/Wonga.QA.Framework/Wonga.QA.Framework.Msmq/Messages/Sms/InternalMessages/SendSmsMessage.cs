using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages
{
    /// <summary> Wonga.Sms.InternalMessages.SendSmsMessage </summary>
    [XmlRoot("SendSmsMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "")]
    public partial class SendSmsMessage : MsmqMessage<SendSmsMessage>
    {
        public Guid SmsMessageId { get; set; }
        public String Text { get; set; }
        public String FormattedPhoneNumber { get; set; }
    }
}
