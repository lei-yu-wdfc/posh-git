using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationDetailsAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationDetailsAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationDetailsAddedInternalEvent : MsmqMessage<IOrganisationDetailsAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
