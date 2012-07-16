using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.AutomaticCardPingRequiredMessage </summary>
    [XmlRoot("AutomaticCardPingRequiredMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class AutomaticCardPingRequiredMessage : MsmqMessage<AutomaticCardPingRequiredMessage>
    {
        public Int32 ApplicationId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public Guid AccountGuid { get; set; }
        public Boolean TryAllCards { get; set; }
    }
}
