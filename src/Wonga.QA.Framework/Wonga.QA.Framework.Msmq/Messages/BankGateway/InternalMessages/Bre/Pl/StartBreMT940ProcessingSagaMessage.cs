using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bre.Pl
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bre.Pl.StartBreMT940ProcessingSagaMessage </summary>
    [XmlRoot("StartBreMT940ProcessingSagaMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bre.Pl", DataType = "")]
    public partial class StartBreMT940ProcessingSagaMessage : MsmqMessage<StartBreMT940ProcessingSagaMessage>
    {
        public DateTime LastBreFileTime { get; set; }
        public Int32 AcknowledgeTypeId { get; set; }
    }
}
