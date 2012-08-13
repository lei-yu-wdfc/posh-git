using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.InternalMessages
{
    /// <summary> Wonga.Sms.InternalMessages.SendSmsToAccountMessage </summary>
    [XmlRoot("SendSmsToAccountMessage", Namespace = "Wonga.Sms.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Sms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendSmsToAccountMessage : MsmqMessage<SendSmsToAccountMessage>
    {
        public Guid SmsMessageId { get; set; }
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
    }
}
