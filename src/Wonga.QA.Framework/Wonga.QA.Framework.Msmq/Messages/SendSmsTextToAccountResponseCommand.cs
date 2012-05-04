using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Sms.InternalMessages.SendSmsTextToAccountResponseMessage </summary>
    [XmlRoot("SendSmsTextToAccountResponseMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "")]
    public partial class SendSmsTextToAccountResponseCommand : MsmqMessage<SendSmsTextToAccountResponseCommand>
    {
        public Guid SmsMessageId { get; set; }
        public Boolean Successful { get; set; }
        public String ErrorMessage { get; set; }
        public String ProviderSmsId { get; set; }
    }
}
