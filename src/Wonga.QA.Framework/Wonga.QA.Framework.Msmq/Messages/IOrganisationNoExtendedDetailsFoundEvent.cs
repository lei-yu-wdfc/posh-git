using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.IOrganisationNoExtendedDetailsFound </summary>
    [XmlRoot("IOrganisationNoExtendedDetailsFound", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationNoExtendedDetailsFoundEvent : MsmqMessage<IOrganisationNoExtendedDetailsFoundEvent>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
