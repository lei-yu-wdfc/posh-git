using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Sms.Data;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages
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
