using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.Commands
{
    /// <summary> Wonga.Comms.ContactManagement.Commands.SaveOrganisationDetailsMessage </summary>
    [XmlRoot("SaveOrganisationDetailsMessage", Namespace = "Wonga.Comms.ContactManagement.Commands", DataType = "")]
    public partial class SaveOrganisationDetailsCommand : MsmqMessage<SaveOrganisationDetailsCommand>
    {
        public Guid OrganisationId { get; set; }
        public String OrganisationName { get; set; }
        public String RegisteredNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
