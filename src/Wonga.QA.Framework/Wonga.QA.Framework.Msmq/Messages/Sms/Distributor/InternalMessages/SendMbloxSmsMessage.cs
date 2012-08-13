using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Sms.Distributor.InternalMessages
{
    /// <summary> Wonga.Sms.Distributor.InternalMessages.SendMbloxSmsMessage </summary>
    [XmlRoot("SendMbloxSmsMessage", Namespace = "Wonga.Sms.Distributor.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Sms.Distributor.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendMbloxSmsMessage : MsmqMessage<SendMbloxSmsMessage>
    {
        public String ToNumberFormatted { get; set; }
        public String MessageText { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
