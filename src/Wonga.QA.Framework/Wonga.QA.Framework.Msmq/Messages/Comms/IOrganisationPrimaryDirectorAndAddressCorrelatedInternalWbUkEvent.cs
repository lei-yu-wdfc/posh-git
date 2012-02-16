using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IOrganisationPrimaryDirectorAndAddressCorrelatedInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IOrganisationPrimaryDirectorAndAddressCorrelated,Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IOrganisationPrimaryDirectorAndAddressCorrelatedInternalWbUkEvent : MsmqMessage<IOrganisationPrimaryDirectorAndAddressCorrelatedInternalWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
