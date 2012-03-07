using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.Ca.SendCollatedLegalDocumentsAtApprovalMessage </summary>
    [XmlRoot("SendCollatedLegalDocumentsAtApprovalMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Ca", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendCollatedLegalDocumentsAtApprovalCaCommand : MsmqMessage<SendCollatedLegalDocumentsAtApprovalCaCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid LoanAgreementFileId { get; set; }
        public Guid PreAuthoriseDirectDebitFileId { get; set; }
        public Guid LoanAgreementCancellationNoticeFileId { get; set; }
        public Guid ConfirmationOfPADFileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
