using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk.CompilePrimaryDirectorCollatedSignedDocumentsEmail </summary>
    [XmlRoot("CompilePrimaryDirectorCollatedSignedDocumentsEmail", Namespace = "Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk", DataType = "")]
    public partial class CompilePrimaryDirectorCollatedSignedDocumentsEmailWbUkEvent : MsmqMessage<CompilePrimaryDirectorCollatedSignedDocumentsEmailWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid SignedLegalAgreementFileId { get; set; }
        public Guid PersonalGuaranteeFileId { get; set; }
    }
}
