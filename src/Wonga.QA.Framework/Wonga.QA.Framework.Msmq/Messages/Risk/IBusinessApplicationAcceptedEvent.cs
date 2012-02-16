using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IBusinessApplicationAccepted", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IAcceptedDecision,Wonga.Risk.IApplicationDecision")]
    public partial class IBusinessApplicationAcceptedEvent : MsmqMessage<IBusinessApplicationAcceptedEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
