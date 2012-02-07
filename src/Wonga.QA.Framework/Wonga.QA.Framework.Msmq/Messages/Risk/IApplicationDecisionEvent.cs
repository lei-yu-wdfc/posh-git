using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IApplicationDecision", Namespace = "Wonga.Risk", DataType = "")]
    public partial class IApplicationDecisionEvent : MsmqMessage<IApplicationDecisionEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
