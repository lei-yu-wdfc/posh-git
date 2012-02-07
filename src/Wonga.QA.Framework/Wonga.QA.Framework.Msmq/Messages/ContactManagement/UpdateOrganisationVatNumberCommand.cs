using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ContactManagement
{
    [XmlRoot("UpdateOrganisationVatNumberMessage", Namespace = "Wonga.Comms.ContactManagement.InternalMessages", DataType = "")]
    public partial class UpdateOrganisationVatNumberCommand : MsmqMessage<UpdateOrganisationVatNumberCommand>
    {
        public Guid OrganisationId { get; set; }
        public String VatNumber { get; set; }
    }
}
