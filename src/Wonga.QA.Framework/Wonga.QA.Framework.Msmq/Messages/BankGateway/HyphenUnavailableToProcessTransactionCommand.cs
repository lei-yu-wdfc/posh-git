using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("HyphenUnavailableToProcessTransaction", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class HyphenUnavailableToProcessTransactionCommand : MsmqMessage<HyphenUnavailableToProcessTransactionCommand>
    {
        public Int32 TransactionId { get; set; }
    }
}
