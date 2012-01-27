using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IApplicationBehaviorAdded", Namespace = "Wonga.Risk.Workflow.Messages", DataType = "")]
    public class IApplicationBehaviorAddedEvent : MsmqMessage<IApplicationBehaviorAddedEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
