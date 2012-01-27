using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CancelLoanApplicationMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class CancelLoanApplicationCommand : MsmqMessage<CancelLoanApplicationCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
