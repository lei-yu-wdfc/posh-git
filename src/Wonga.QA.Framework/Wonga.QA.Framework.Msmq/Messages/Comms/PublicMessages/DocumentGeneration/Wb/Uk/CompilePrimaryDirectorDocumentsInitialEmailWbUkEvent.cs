using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.DocumentGeneration.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk.CompilePrimaryDirectorDocumentsInitialEmail </summary>
    [XmlRoot("CompilePrimaryDirectorDocumentsInitialEmail", Namespace = "Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk", DataType = "")]
    public partial class CompilePrimaryDirectorDocumentsInitialEmailWbUkEvent : MsmqMessage<CompilePrimaryDirectorDocumentsInitialEmailWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid LoanAgreementFileId { get; set; }
        public Guid PersonalGuaranteeFileId { get; set; }
    }
}
