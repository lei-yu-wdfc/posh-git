using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ProcessReturnedPaymentTransactionMessage </summary>
    [XmlRoot("ProcessReturnedPaymentTransactionMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ProcessReturnedPaymentTransactionCommand : MsmqMessage<ProcessReturnedPaymentTransactionCommand>
    {
        public Guid ApplicationId { get; set; }
    }
}
