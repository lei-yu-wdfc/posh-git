using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Ca.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Ca.Instructions.IWantToGenerateALoanAgreementDocumentResponse </summary>
    [XmlRoot("IWantToGenerateALoanAgreementDocumentResponse", Namespace = "Wonga.PublicMessages.Comms.Ca.Instructions", DataType = "Wonga.PublicMessages.Comms.Instructions.IWantToGenerateALegalDocumentResponse")]
    public partial class IWantToGenerateALoanAgreementDocumentResponse : MsmqMessage<IWantToGenerateALoanAgreementDocumentResponse>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
    }
}
