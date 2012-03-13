using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement.CreateAndStoreFullySignedLegalAgreementDocumentMessage </summary>
    [XmlRoot("CreateAndStoreFullySignedLegalAgreementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement", DataType = "")]
    public partial class CreateAndStoreFullySignedLegalAgreementDocumentWbUkCommand : MsmqMessage<CreateAndStoreFullySignedLegalAgreementDocumentWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
