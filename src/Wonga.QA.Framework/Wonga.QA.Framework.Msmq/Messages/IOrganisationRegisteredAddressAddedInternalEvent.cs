using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationRegisteredAddressAddedInternal </summary>
    [XmlRoot("IOrganisationRegisteredAddressAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationRegisteredAddressAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationRegisteredAddressAddedInternalEvent : MsmqMessage<IOrganisationRegisteredAddressAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
