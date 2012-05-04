using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.FailedPaymentMessage </summary>
    [XmlRoot("FailedPaymentMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class FailedPaymentCommand : MsmqMessage<FailedPaymentCommand>
    {
        public Int32 TransactionId { get; set; }
        public String ErrorMessage { get; set; }
    }
}