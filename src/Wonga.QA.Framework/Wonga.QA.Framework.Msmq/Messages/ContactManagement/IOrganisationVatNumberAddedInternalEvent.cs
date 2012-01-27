using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationVatNumberAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationVatNumberAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public class IOrganisationVatNumberAddedInternalEvent : MsmqMessage<IOrganisationVatNumberAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
