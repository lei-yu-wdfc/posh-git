using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationExtendedDetailsAdded", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public class IOrganisationExtendedDetailsAddedEvent : MsmqMessage<IOrganisationExtendedDetailsAddedEvent>
    {
        public Guid OrganisationId { get; set; }
        public String ExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
