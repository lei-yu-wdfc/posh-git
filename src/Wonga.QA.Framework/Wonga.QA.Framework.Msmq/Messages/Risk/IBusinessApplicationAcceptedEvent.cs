using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.Business.IBusinessApplicationAccepted </summary>
    [XmlRoot("IBusinessApplicationAccepted", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IApplicationAccepted,Wonga.Risk.IAcceptedDecision,Wonga.Risk.IDecisionMessage,Wonga.Risk.IRiskEvent,Wonga.Risk.IApplicationDecision")]
    public partial class IBusinessApplicationAcceptedEvent : MsmqMessage<IBusinessApplicationAcceptedEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
