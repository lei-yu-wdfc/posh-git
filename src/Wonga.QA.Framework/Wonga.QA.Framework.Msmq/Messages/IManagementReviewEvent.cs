using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.IManagementReview </summary>
    [XmlRoot("IManagementReview", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IManagementReviewEvent : MsmqMessage<IManagementReviewEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
    }
}
