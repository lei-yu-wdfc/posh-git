using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.HyphenAvailableToProcessTransaction </summary>
    [XmlRoot("HyphenAvailableToProcessTransaction", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class HyphenAvailableToProcessTransactionCommand : MsmqMessage<HyphenAvailableToProcessTransactionCommand>
    {
        public Int32 TransactionId { get; set; }
    }
}
