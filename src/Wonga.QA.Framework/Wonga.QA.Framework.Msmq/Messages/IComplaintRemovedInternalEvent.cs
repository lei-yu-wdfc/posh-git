using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.IComplaintRemovedInternal </summary>
    [XmlRoot("IComplaintRemovedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IComplaintRemoved")]
    public partial class IComplaintRemovedInternalEvent : MsmqMessage<IComplaintRemovedInternalEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
    }
}
