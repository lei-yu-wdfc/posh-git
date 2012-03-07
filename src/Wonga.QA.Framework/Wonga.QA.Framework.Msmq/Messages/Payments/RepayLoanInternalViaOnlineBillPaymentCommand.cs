using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("RepayLoanInternalViaOnlineBillPayment", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class RepayLoanInternalViaOnlineBillPaymentCommand : MsmqMessage<RepayLoanInternalViaOnlineBillPaymentCommand>
    {
        public Decimal PayorAmount { get; set; }
        public String PayorName { get; set; }
        public String PayorAccountNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public String RemittanceTraceNumber { get; set; }
    }
}
