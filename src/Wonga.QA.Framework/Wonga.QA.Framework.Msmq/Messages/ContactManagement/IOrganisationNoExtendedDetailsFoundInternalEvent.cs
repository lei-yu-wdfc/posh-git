using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("IOrganisationNoExtendedDetailsFoundInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationNoExtendedDetailsFound,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationNoExtendedDetailsFoundInternalEvent : MsmqMessage<IOrganisationNoExtendedDetailsFoundInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
