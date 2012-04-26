using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ReverseCashAdvanceTransactionMessage </summary>
    [XmlRoot("ReverseCashAdvanceTransactionMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ReverseCashAdvanceTransactionCommand : MsmqMessage<ReverseCashAdvanceTransactionCommand>
    {
        public Guid ApplicationId { get; set; }
    }
}
