using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ProcessBounceTransactionMessage </summary>
    [XmlRoot("ProcessBounceTransactionMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ProcessBounceTransactionCommand : MsmqMessage<ProcessBounceTransactionCommand>
    {
        public Guid ApplicationId { get; set; }
    }
}