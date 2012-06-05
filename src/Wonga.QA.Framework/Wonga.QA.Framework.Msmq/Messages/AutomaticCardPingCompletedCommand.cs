using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AutomaticCardPingCompletedMessage </summary>
    [XmlRoot("AutomaticCardPingCompletedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AutomaticCardPingCompletedCommand : MsmqMessage<AutomaticCardPingCompletedCommand>
    {
        public Guid ApplicationGuid { get; set; }
        public AutomaticPingStatusEnum Status { get; set; }
    }
}
