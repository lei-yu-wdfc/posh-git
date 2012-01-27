using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("FailedPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public class FailedPaymentCommand : MsmqMessage<FailedPaymentCommand>
    {
        public Int32 TransactionId { get; set; }
        public String ErrorMessage { get; set; }
    }
}
