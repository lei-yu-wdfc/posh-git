using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStorePersonalGuaranteeDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration", DataType = "")]
    public class CreateAndStorePersonalGuaranteeDocumentCompleteCommand : MsmqMessage<CreateAndStorePersonalGuaranteeDocumentCompleteCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
    }
}
