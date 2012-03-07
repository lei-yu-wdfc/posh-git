using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Business.IBusinessApplicationDeclined </summary>
    [XmlRoot("IBusinessApplicationDeclined", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IApplicationDeclined,Wonga.Risk.IDeclinedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent,Wonga.Risk.IApplicationDecision")]
    public partial class IBusinessApplicationDeclinedEvent : MsmqMessage<IBusinessApplicationDeclinedEvent>
    {
        public Guid OrganisationId { get; set; }
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
