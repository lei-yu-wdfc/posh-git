using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("StopPingMessage", Namespace = "Wonga.Payments.InternalMessages.PingEngine", DataType = "")]
    public class StopPingCommand : MsmqMessage<StopPingCommand>
    {
        public Guid AccoundId { get; set; }
    }
}
