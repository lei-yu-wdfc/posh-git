using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IDeclinedDecision", Namespace = "Wonga.Risk", DataType = "")]
    public class IDeclinedDecisionEvent : MsmqMessage<IDeclinedDecisionEvent>
    {
        public String FailedCheckpointName { get; set; }
    }
}
