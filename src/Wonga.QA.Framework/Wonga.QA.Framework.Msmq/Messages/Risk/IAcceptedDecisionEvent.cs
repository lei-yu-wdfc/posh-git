using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IAcceptedDecision", Namespace = "Wonga.Risk", DataType = "")]
    public class IAcceptedDecisionEvent : MsmqMessage<IAcceptedDecisionEvent>
    {
    }
}
