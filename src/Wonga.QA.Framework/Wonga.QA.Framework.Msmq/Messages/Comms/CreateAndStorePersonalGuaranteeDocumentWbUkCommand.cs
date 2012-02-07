using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStorePersonalGuaranteeDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee", DataType = "")]
    public partial class CreateAndStorePersonalGuaranteeDocumentWbUkCommand : MsmqMessage<CreateAndStorePersonalGuaranteeDocumentWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
