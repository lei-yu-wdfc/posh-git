using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Sms
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SendSimpleSmsMessage </summary>
    [XmlRoot("SendSimpleSmsMessage", Namespace = "Wonga.Comms.InternalMessages.Sms", DataType = "")]
    public partial class SendSimpleSmsCommand : MsmqMessage<SendSimpleSmsCommand>
    {
        public String ToNumber { get; set; }
        public String MessageText { get; set; }
    }
}
