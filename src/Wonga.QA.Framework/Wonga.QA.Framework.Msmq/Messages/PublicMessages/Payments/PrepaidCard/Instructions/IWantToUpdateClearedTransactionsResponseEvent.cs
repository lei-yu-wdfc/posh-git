using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateClearedTransactionsResponse </summary>
    [XmlRoot("IWantToUpdateClearedTransactionsResponse", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToUpdateClearedTransactionsResponseEvent : MsmqMessage<IWantToUpdateClearedTransactionsResponseEvent>
    {
        public Object ClearedTransactions { get; set; }
    }
}
