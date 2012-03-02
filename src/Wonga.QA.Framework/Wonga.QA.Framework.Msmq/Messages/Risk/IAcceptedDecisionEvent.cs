using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IAcceptedDecision", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IApplicationDecision,Wonga.Risk.IRiskEvent")]
    public partial class IAcceptedDecisionEvent : MsmqMessage<IAcceptedDecisionEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
