using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Workflow.Messages.IApplicationBehaviorAdded </summary>
    [XmlRoot("IApplicationBehaviorAdded", Namespace = "Wonga.Risk.Workflow.Messages", DataType = "")]
    public partial class IApplicationBehaviorAddedEvent : MsmqMessage<IApplicationBehaviorAddedEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
