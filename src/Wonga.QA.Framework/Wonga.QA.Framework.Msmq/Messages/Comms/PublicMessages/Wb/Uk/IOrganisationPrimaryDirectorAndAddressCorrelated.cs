using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IOrganisationPrimaryDirectorAndAddressCorrelated </summary>
    [XmlRoot("IOrganisationPrimaryDirectorAndAddressCorrelated", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IOrganisationPrimaryDirectorAndAddressCorrelated : MsmqMessage<IOrganisationPrimaryDirectorAndAddressCorrelated>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
