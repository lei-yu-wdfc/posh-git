using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationSecondaryDirectorAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationSecondaryDirectorAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public class IOrganisationSecondaryDirectorAddedInternalEvent : MsmqMessage<IOrganisationSecondaryDirectorAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
