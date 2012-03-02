using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IBusinessApplicationDeclined", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IDeclinedDecision,Wonga.Risk.IApplicationDecision,Wonga.Risk.IRiskEvent")]
    public partial class IBusinessApplicationDeclinedEvent : MsmqMessage<IBusinessApplicationDeclinedEvent>
    {
        public Guid OrganisationId { get; set; }
        public String FailedCheckpointName { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
