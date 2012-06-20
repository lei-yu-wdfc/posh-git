using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages.Events
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.Events.IOrganisationExtendedDetailsAddedInternal </summary>
    [XmlRoot("IOrganisationExtendedDetailsAddedInternal", Namespace = "Wonga.Comms.ContactManagement.InternalMessages.Events", DataType = "Wonga.Comms.ContactManagement.PublicMessages.IOrganisationExtendedDetailsAdded,Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationExtendedDetailsAddedInternalEvent : MsmqMessage<IOrganisationExtendedDetailsAddedInternalEvent>
    {
        public Guid OrganisationId { get; set; }
        public String ExternalId { get; set; }
        public Boolean TradingAddressAvailable { get; set; }
        public Boolean RegisteredAddressAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
