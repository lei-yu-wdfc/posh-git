using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement.CreateAndStoreUnsignedLegalAgreementDocumentMessage </summary>
    [XmlRoot("CreateAndStoreUnsignedLegalAgreementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement", DataType = "")]
    public partial class CreateAndStoreUnsignedLegalAgreementDocumentMessage : MsmqMessage<CreateAndStoreUnsignedLegalAgreementDocumentMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
