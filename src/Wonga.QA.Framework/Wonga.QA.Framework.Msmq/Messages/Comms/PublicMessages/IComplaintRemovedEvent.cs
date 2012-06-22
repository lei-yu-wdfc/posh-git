using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IComplaintRemoved </summary>
    [XmlRoot("IComplaintRemoved", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IComplaintRemovedEvent : MsmqMessage<IComplaintRemovedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
    }
}
