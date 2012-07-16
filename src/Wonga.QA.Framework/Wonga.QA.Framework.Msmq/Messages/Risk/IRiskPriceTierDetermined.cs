using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskPriceTierDetermined </summary>
    [XmlRoot("IRiskPriceTierDetermined", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IPriceTierDetermined,Wonga.Risk.IRiskEvent")]
    public partial class IRiskPriceTierDetermined : MsmqMessage<IRiskPriceTierDetermined>
    {
        public RiskPriceTierEnum RiskPriceTier { get; set; }
        public Guid ApplicationId { get; set; }
        public UInt32 Tier { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
