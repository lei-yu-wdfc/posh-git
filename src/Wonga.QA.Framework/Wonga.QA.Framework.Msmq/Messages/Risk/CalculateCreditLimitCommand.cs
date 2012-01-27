using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("CalculateCreditLimitMessage", Namespace = "Wonga.Risk", DataType = "")]
    public class CalculateCreditLimitCommand : MsmqMessage<CalculateCreditLimitCommand>
    {
        public Guid AccountId { get; set; }
    }
}
