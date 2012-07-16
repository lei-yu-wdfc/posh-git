using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.IOrganisationExternalDataRetrieved </summary>
    [XmlRoot("IOrganisationExternalDataRetrieved", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationExternalDataRetrieved : MsmqMessage<IOrganisationExternalDataRetrieved>
    {
        public Guid OrganisationId { get; set; }
        public String ExternalId { get; set; }
        public Boolean TradingAddressAvailable { get; set; }
        public Boolean RegisteredAddressAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
