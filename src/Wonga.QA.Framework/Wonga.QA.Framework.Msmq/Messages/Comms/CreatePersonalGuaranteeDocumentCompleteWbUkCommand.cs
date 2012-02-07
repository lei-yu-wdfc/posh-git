using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreatePersonalGuaranteeDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PersonalGuarantee", DataType = "")]
    public partial class CreatePersonalGuaranteeDocumentCompleteWbUkCommand : MsmqMessage<CreatePersonalGuaranteeDocumentCompleteWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Byte[] Content { get; set; }
    }
}
