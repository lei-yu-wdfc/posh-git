using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.PingEngine.StopPingMessage </summary>
    [XmlRoot("StopPingMessage", Namespace = "Wonga.Payments.InternalMessages.PingEngine", DataType = "")]
    public partial class StopPingCommand : MsmqMessage<StopPingCommand>
    {
        public Guid AccoundId { get; set; }
    }
}
