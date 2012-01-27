using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IOrganisationPrimaryDirectorAndAddressAdded", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public class IOrganisationPrimaryDirectorAndAddressAddedWbUkEvent : MsmqMessage<IOrganisationPrimaryDirectorAndAddressAddedWbUkEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
