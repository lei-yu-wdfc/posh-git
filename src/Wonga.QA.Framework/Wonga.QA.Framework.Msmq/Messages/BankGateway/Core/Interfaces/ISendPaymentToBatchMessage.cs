using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.Core.Interfaces
{
    /// <summary> Wonga.BankGateway.Core.Interfaces.ISendPaymentToBatchMessage </summary>
    [XmlRoot("ISendPaymentToBatchMessage", Namespace = "Wonga.BankGateway.Core.Interfaces", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ISendPaymentToBatchMessage : MsmqMessage<ISendPaymentToBatchMessage>
    {
        public Guid BatchQueueId { get; set; }
        public Int32 TransactionId { get; set; }
    }
}
