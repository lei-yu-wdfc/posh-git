using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IComplaintReported </summary>
    [XmlRoot("IComplaintReported", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IComplaintReportedEvent : MsmqMessage<IComplaintReportedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
    }
}
