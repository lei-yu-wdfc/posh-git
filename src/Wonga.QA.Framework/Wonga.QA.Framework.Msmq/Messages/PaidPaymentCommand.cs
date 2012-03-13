using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.PaidPaymentMessage </summary>
    [XmlRoot("PaidPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class PaidPaymentCommand : MsmqMessage<PaidPaymentCommand>
    {
        public Int32 TransactionId { get; set; }
    }
}
