using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreLegalAgreementDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement", DataType = "")]
    public partial class CreateAndStoreLegalAgreementDocumentCompleteWbUkCommand : MsmqMessage<CreateAndStoreLegalAgreementDocumentCompleteWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
    }
}
