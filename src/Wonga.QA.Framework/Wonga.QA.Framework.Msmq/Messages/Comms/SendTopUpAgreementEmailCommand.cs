using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendTopUpAgreementEmailMessage </summary>
    [XmlRoot("SendTopUpAgreementEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendTopUpAgreementEmailCommand : MsmqMessage<SendTopUpAgreementEmailCommand>
    {
        public String Email { get; set; }
        public String Forename { get; set; }
        public Guid AgreementFileId { get; set; }
        public String PageUrl { get; set; }
        public String PromiseDate { get; set; }
        public String LoanAmount { get; set; }
        public String BorrowedTotal { get; set; }
        public String TotalRepayable { get; set; }
        public Guid SagaId { get; set; }
    }
}
