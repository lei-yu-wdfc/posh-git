using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.IRiskPriceTierDetermined </summary>
    [XmlRoot("IRiskPriceTierDetermined", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IPriceTierDetermined,Wonga.Risk.IRiskEvent")]
    public partial class IRiskPriceTierDeterminedEvent : MsmqMessage<IRiskPriceTierDeterminedEvent>
    {
        public RiskPriceTierEnum RiskPriceTier { get; set; }
        public Guid ApplicationId { get; set; }
        public UInt32 Tier { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
