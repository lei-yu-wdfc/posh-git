using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("RepayLoanInternalViaBank", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class RepayLoanInternalViaBankCommand : MsmqMessage<RepayLoanInternalViaBankCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid? CashEntityId { get; set; }
        public Decimal Amount { get; set; }
        public Guid RepaymentRequestId { get; set; }
    }
}
