using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.InternalMessages.PingEngine.StartPingMessage </summary>
    [XmlRoot("StartPingMessage", Namespace = "Wonga.Payments.InternalMessages.PingEngine", DataType = "")]
    public partial class StartPingCommand : MsmqMessage<StartPingCommand>
    {
        public Guid AccoundId { get; set; }
    }
}
