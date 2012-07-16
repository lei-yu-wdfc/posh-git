using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ReverseCashAdvanceTransactionMessage </summary>
    [XmlRoot("ReverseCashAdvanceTransactionMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ReverseCashAdvanceTransactionMessage : MsmqMessage<ReverseCashAdvanceTransactionMessage>
    {
        public Guid ApplicationId { get; set; }
    }
}
