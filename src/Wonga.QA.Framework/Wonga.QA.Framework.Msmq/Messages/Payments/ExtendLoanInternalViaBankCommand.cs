using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ExtendLoanInternalViaBank", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class ExtendLoanInternalViaBankCommand : MsmqMessage<ExtendLoanInternalViaBankCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid? CashEntityId { get; set; }
        public DateTime NextDueDate { get; set; }
        public Guid RepaymentRequestId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
