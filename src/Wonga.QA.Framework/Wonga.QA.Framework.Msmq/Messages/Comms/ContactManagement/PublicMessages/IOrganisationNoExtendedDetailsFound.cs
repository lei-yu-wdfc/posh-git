using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.IOrganisationNoExtendedDetailsFound </summary>
    [XmlRoot("IOrganisationNoExtendedDetailsFound", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationNoExtendedDetailsFound : MsmqMessage<IOrganisationNoExtendedDetailsFound>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
