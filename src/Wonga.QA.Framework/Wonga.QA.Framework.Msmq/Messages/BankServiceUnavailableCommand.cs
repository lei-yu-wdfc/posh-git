using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.BankServiceUnavailableMessage </summary>
    [XmlRoot("BankServiceUnavailableMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class BankServiceUnavailableCommand : MsmqMessage<BankServiceUnavailableCommand>
    {
        public Int32 BankIntegrationId { get; set; }
        public Int32 TransactionId { get; set; }
        public Int32 DirectDebitId { get; set; }
    }
}
