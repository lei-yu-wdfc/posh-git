using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RepayLoanInternalViaOnlineBillPayment </summary>
    [XmlRoot("RepayLoanInternalViaOnlineBillPayment", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class RepayLoanInternalViaOnlineBillPayment : MsmqMessage<RepayLoanInternalViaOnlineBillPayment>
    {
        public Decimal Amount { get; set; }
        public String CustomerFullName { get; set; }
        public String Ccin { get; set; }
        public DateTime PaymentDate { get; set; }
        public String RemittanceTraceNumber { get; set; }
    }
}
