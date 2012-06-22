using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.IManagementReviewRemovedInternal </summary>
    [XmlRoot("IManagementReviewRemovedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IManagementReviewRemoved")]
    public partial class IManagementReviewRemovedInternalEvent : MsmqMessage<IManagementReviewRemovedInternalEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
    }
}
