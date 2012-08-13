using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages
{
    /// <summary> Wonga.Sms.InternalMessages.SendSmsResponseMessage </summary>
    [XmlRoot("SendSmsResponseMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Sms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendSmsResponseMessage : MsmqMessage<SendSmsResponseMessage>
    {
        public Guid SmsMessageId { get; set; }
        public Boolean Successful { get; set; }
        public String ErrorMessage { get; set; }
        public String ProviderSmsId { get; set; }
    }
}
