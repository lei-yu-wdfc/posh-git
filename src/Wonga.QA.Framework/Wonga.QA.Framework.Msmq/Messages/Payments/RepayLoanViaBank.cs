using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.RepayLoanViaBank </summary>
    [XmlRoot("RepayLoanViaBank", Namespace = "Wonga.Payments", DataType = "")]
    public partial class RepayLoanViaBank : MsmqMessage<RepayLoanViaBank>
    {
        public Guid ApplicationId { get; set; }
        public Guid? CashEntityId { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime ActionDate { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
