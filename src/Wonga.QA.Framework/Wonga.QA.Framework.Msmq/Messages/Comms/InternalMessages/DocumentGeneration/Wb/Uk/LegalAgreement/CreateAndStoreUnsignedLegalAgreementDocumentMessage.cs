using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement.CreateAndStoreUnsignedLegalAgreementDocumentMessage </summary>
    [XmlRoot("CreateAndStoreUnsignedLegalAgreementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.LegalAgreement", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAndStoreUnsignedLegalAgreementDocumentMessage : MsmqMessage<CreateAndStoreUnsignedLegalAgreementDocumentMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
