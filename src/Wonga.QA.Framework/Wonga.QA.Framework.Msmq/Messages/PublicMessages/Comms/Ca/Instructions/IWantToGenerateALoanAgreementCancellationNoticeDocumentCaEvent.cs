using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Ca.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Ca.Instructions.IWantToGenerateALoanAgreementCancellationNoticeDocument </summary>
    [XmlRoot("IWantToGenerateALoanAgreementCancellationNoticeDocument", Namespace = "Wonga.PublicMessages.Comms.Ca.Instructions", DataType = "Wonga.PublicMessages.Comms.Instructions.IWantToGenerateALegalDocument")]
    public partial class IWantToGenerateALoanAgreementCancellationNoticeDocumentCaEvent : MsmqMessage<IWantToGenerateALoanAgreementCancellationNoticeDocumentCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
