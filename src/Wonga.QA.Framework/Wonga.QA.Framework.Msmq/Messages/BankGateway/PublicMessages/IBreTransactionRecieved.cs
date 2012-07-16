using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.PublicMessages
{
    /// <summary> Wonga.BankGateway.PublicMessages.IBreTransactionRecieved </summary>
    [XmlRoot("IBreTransactionRecieved", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "")]
    public partial class IBreTransactionRecieved : MsmqMessage<IBreTransactionRecieved>
    {
        public DateTime CreatedOn { get; set; }
        public String IDIPH { get; set; }
        public Decimal Amount { get; set; }
        public String Currency { get; set; }
    }
}
