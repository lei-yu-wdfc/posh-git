using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("SaveOrganisationDetailsMessage", Namespace = "Wonga.Comms.ContactManagement.Commands", DataType = "")]
    public class SaveOrganisationDetailsCommand : MsmqMessage<SaveOrganisationDetailsCommand>
    {
        public Guid OrganisationId { get; set; }
        public String OrganisationName { get; set; }
        public String RegisteredNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
