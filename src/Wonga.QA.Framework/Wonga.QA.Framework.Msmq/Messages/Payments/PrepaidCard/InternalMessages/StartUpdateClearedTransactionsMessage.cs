using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.StartUpdateClearedTransactionsMessage </summary>
    [XmlRoot("StartUpdateClearedTransactionsMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "")]
    public partial class StartUpdateClearedTransactionsMessage : MsmqMessage<StartUpdateClearedTransactionsMessage>
    {
        public Guid Id { get; set; }
    }
}
