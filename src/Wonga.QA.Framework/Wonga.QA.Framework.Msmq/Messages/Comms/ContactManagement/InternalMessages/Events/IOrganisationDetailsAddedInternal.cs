using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages.Events
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationDetailsAddedInternal </summary>
    [XmlRoot("IOrganisationDetailsAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationDetailsAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationDetailsAddedInternal : MsmqMessage<IOrganisationDetailsAddedInternal>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
