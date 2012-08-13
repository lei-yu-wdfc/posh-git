using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.DocumentGeneration.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk.CompileGuarantorDocumentsInitialEmail </summary>
    [XmlRoot("CompileGuarantorDocumentsInitialEmail", Namespace = "Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompileGuarantorDocumentsInitialEmail : MsmqMessage<CompileGuarantorDocumentsInitialEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid PersonalGuaranteeFileId { get; set; }
        public Guid PrimaryApplicantLoanAgreementDocumentId { get; set; }
        public String ReturnLinkCorrelationId { get; set; }
    }
}
