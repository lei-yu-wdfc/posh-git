using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ManualReverseCashAdvanceTransactionMessage </summary>
    [XmlRoot("ManualReverseCashAdvanceTransactionMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ManualReverseCashAdvanceTransactionMessage : MsmqMessage<ManualReverseCashAdvanceTransactionMessage>
    {
        public Guid ApplicationId { get; set; }
    }
}
