using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationExtendedDetailsAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationExtendedDetailsAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public class IOrganisationExtendedDetailsAddedInternalEvent : MsmqMessage<IOrganisationExtendedDetailsAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public String ExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
