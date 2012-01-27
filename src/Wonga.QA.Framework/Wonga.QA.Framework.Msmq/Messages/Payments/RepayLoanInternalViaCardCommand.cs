using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("RepayLoanInternalViaCard", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class RepayLoanInternalViaCardCommand : MsmqMessage<RepayLoanInternalViaCardCommand>
    {
        public Object PaymentCardCv2 { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? CashEntityId { get; set; }
        public Decimal Amount { get; set; }
        public Guid RepaymentRequestId { get; set; }
    }
}
