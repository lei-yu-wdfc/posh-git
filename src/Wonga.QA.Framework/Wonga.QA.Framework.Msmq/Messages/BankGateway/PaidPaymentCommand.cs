using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("PaidPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public class PaidPaymentCommand : MsmqMessage<PaidPaymentCommand>
    {
        public Int32 TransactionId { get; set; }
    }
}
