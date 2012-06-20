using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Pl
{
    /// <summary> Wonga.BankGateway.InternalMessages.Pl.BaseBreTransactionMessage </summary>
    [XmlRoot("BaseBreTransactionMessage", Namespace = "Wonga.BankGateway.InternalMessages.Pl", DataType = "")]
    public partial class BaseBreTransactionPlCommand : MsmqMessage<BaseBreTransactionPlCommand>
    {
        public DateTime LastBreFileTime { get; set; }
        public Int32 AcknowledgeTypeId { get; set; }
    }
}
