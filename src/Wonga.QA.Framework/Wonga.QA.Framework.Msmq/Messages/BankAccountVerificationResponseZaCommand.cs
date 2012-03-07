using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Hyphen.BankAccountVerificationResponseMessage </summary>
    [XmlRoot("BankAccountVerificationResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Hyphen", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class BankAccountVerificationResponseZaCommand : MsmqMessage<BankAccountVerificationResponseZaCommand>
    {
        public Guid ApplicationId { get; set; }
        public Boolean IsTimeout { get; set; }
        public Boolean IsValidAccountNumber { get; set; }
        public Boolean IsMatchNationalNumber { get; set; }
        public String ErrorMessage { get; set; }
        public Boolean? IsActiveMoreThanThreeMonths { get; set; }
        public Boolean? AcceptsCredits { get; set; }
        public Boolean? AcceptsDebits { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
