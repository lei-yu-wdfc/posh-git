using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.DocumentGeneration.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk.CompilePrimaryDirectorDocumentsInitialEmail </summary>
    [XmlRoot("CompilePrimaryDirectorDocumentsInitialEmail", Namespace = "Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.DocumentGeneration.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompilePrimaryDirectorDocumentsInitialEmail : MsmqMessage<CompilePrimaryDirectorDocumentsInitialEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid LoanAgreementFileId { get; set; }
        public Guid PersonalGuaranteeFileId { get; set; }
    }
}
