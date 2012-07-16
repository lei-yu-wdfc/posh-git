using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages.CorrelationEvents
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents.IApplicationOrganisationCorrelated </summary>
    [XmlRoot("IApplicationOrganisationCorrelated", Namespace = "Wonga.Comms.ContactManagement.PublicMessages.CorrelationEvents", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IApplicationOrganisationCorrelated : MsmqMessage<IApplicationOrganisationCorrelated>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
