using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("RepayLoanViaCard", Namespace = "Wonga.Payments", DataType = "")]
    public partial class RepayLoanViaCardCommand : MsmqMessage<RepayLoanViaCardCommand>
    {
        public String PaymentCardCv2 { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? CashEntityId { get; set; }
        public Decimal? Amount { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
