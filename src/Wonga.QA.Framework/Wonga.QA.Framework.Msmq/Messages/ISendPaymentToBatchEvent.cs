using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.Core.Interfaces.ISendPaymentToBatchMessage </summary>
    [XmlRoot("ISendPaymentToBatchMessage", Namespace = "Wonga.BankGateway.Core.Interfaces", DataType = "")]
    public partial class ISendPaymentToBatchEvent : MsmqMessage<ISendPaymentToBatchEvent>
    {
        public Guid BatchQueueId { get; set; }
        public Int32 TransactionId { get; set; }
    }
}
