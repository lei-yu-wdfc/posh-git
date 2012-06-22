using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.HyphenUnavailableToProcessTransaction </summary>
    [XmlRoot("HyphenUnavailableToProcessTransaction", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class HyphenUnavailableToProcessTransactionCommand : MsmqMessage<HyphenUnavailableToProcessTransactionCommand>
    {
        public Int32 TransactionId { get; set; }
    }
}
