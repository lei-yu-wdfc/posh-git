using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.IOrganisationTradingAddressAdded </summary>
    [XmlRoot("IOrganisationTradingAddressAdded", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent")]
    public partial class IOrganisationTradingAddressAdded : MsmqMessage<IOrganisationTradingAddressAdded>
    {
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
