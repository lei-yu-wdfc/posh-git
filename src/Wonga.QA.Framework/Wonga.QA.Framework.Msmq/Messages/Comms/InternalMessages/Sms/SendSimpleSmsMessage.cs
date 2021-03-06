using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Sms
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SendSimpleSmsMessage </summary>
    [XmlRoot("SendSimpleSmsMessage", Namespace = "Wonga.Comms.InternalMessages.Sms", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Sms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendSimpleSmsMessage : MsmqMessage<SendSimpleSmsMessage>
    {
        public String ToNumber { get; set; }
        public String MessageText { get; set; }
    }
}
