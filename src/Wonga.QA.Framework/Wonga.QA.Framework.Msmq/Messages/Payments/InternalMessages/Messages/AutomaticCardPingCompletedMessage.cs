using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Messages;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AutomaticCardPingCompletedMessage </summary>
    [XmlRoot("AutomaticCardPingCompletedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AutomaticCardPingCompletedMessage : MsmqMessage<AutomaticCardPingCompletedMessage>
    {
        public Guid ApplicationGuid { get; set; }
        public AutomaticPingStatusEnum Status { get; set; }
    }
}
