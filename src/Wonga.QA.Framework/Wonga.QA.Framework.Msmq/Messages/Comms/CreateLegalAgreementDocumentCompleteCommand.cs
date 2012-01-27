using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateLegalAgreementDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration", DataType = "")]
    public class CreateLegalAgreementDocumentCompleteCommand : MsmqMessage<CreateLegalAgreementDocumentCompleteCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Byte[] Content { get; set; }
    }
}
