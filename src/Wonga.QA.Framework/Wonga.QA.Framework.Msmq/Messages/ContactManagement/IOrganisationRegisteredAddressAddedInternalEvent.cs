using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationRegisteredAddressAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationRegisteredAddressAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public class IOrganisationRegisteredAddressAddedInternalEvent : MsmqMessage<IOrganisationRegisteredAddressAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
