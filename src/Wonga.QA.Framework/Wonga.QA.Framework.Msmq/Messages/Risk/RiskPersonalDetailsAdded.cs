using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.RiskPersonalDetailsAdded </summary>
    [XmlRoot("RiskPersonalDetailsAdded", Namespace = "Wonga.Risk", DataType = "")]
    public partial class RiskPersonalDetailsAdded : MsmqMessage<RiskPersonalDetailsAdded>
    {
        public Guid AccountId { get; set; }
    }
}
