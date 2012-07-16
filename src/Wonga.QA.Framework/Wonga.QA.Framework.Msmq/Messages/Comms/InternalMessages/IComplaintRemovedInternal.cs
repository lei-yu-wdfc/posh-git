using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.IComplaintRemovedInternal </summary>
    [XmlRoot("IComplaintRemovedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IComplaintRemoved")]
    public partial class IComplaintRemovedInternal : MsmqMessage<IComplaintRemovedInternal>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
    }
}
