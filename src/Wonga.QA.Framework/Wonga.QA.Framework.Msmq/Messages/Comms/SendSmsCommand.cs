using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendSmsMessage", Namespace = "Wonga.Comms.InternalMessages.Sms", DataType = "")]
    public partial class SendSmsCommand : MsmqMessage<SendSmsCommand>
    {
        public String ToNumberFormatted { get; set; }
        public String MessageText { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
