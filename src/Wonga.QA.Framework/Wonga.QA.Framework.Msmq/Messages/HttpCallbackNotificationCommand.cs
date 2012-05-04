using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Sms.InternalMessages.HttpCallbackNotificationMessage </summary>
    [XmlRoot("HttpCallbackNotificationMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "")]
    public partial class HttpCallbackNotificationCommand : MsmqMessage<HttpCallbackNotificationCommand>
    {
        public Guid SmsMessageId { get; set; }
        public SmsStatusEnum Status { get; set; }
        public String ProviderStatus { get; set; }
    }
}
