using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages
{
    /// <summary> Wonga.BankGateway.InternalMessages.BankServiceAvailabiltyMessage </summary>
    [XmlRoot("BankServiceAvailabiltyMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class BankServiceAvailabiltyCommand : MsmqMessage<BankServiceAvailabiltyCommand>
    {
        public Int32 TransactionId { get; set; }
        public Int32 DirectDebitId { get; set; }
    }
}
