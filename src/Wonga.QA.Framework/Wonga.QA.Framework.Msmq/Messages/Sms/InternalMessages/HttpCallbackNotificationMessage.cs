using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Sms.Data;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages
{
    /// <summary> Wonga.Sms.InternalMessages.HttpCallbackNotificationMessage </summary>
    [XmlRoot("HttpCallbackNotificationMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Sms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HttpCallbackNotificationMessage : MsmqMessage<HttpCallbackNotificationMessage>
    {
        public Guid SmsMessageId { get; set; }
        public SmsStatusEnum Status { get; set; }
        public String ProviderStatus { get; set; }
    }
}
