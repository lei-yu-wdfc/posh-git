using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Sms
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SendSmsMessage </summary>
    [XmlRoot("SendSmsMessage", Namespace = "Wonga.Comms.InternalMessages.Sms", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Sms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendSmsMessage : MsmqMessage<SendSmsMessage>
    {
        public Guid? AccountId { get; set; }
        public String ToNumberFormatted { get; set; }
        public String MessageText { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
