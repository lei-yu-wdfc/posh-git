using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateLegalAgreementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement", DataType = "")]
    public partial class CreateLegalAgreementDocumentWbUkCommand : MsmqMessage<CreateLegalAgreementDocumentWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
