using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    /// <summary> Wonga.BankGateway.InternalMessages.BankServiceAvailableMessage </summary>
    [XmlRoot("BankServiceAvailableMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class BankServiceAvailableCommand : MsmqMessage<BankServiceAvailableCommand>
    {
        public Int32 BankIntegrationId { get; set; }
        public Int32 TransactionId { get; set; }
        public Int32 DirectDebitId { get; set; }
    }
}
