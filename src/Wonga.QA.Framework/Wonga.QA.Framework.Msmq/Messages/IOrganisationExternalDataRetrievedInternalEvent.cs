using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationExternalDataRetrievedInternal </summary>
    [XmlRoot("IOrganisationExternalDataRetrievedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationExternalDataRetrieved,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationExternalDataRetrievedInternalEvent : MsmqMessage<IOrganisationExternalDataRetrievedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public String ExternalId { get; set; }
        public Boolean TradingAddressAvailable { get; set; }
        public Boolean RegisteredAddressAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
