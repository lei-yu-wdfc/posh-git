using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("StartPingMessage", Namespace = "Wonga.Payments.InternalMessages.PingEngine", DataType = "")]
    public class StartPingCommand : MsmqMessage<StartPingCommand>
    {
        public Guid AccoundId { get; set; }
    }
}
