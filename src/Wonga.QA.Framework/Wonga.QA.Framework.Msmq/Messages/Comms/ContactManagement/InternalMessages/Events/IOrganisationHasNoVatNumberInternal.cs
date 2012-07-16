using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages.Events
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationHasNoVatNumberInternal </summary>
    [XmlRoot("IOrganisationHasNoVatNumberInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationHasNoVatNumber,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationHasNoVatNumberInternal : MsmqMessage<IOrganisationHasNoVatNumberInternal>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
