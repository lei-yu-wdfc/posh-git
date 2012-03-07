using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Standard.Za.SendBatchPaymentsToStandardMessage </summary>
    [XmlRoot("SendBatchPaymentsToStandardMessage", Namespace = "Wonga.BankGateway.InternalMessages.Standard.Za", DataType = "")]
    public partial class SendBatchPaymentsToStandardZaCommand : MsmqMessage<SendBatchPaymentsToStandardZaCommand>
    {
        public List<Int32> TransactionIds { get; set; }
    }
}
