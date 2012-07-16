using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateClearedTransactionsResponse </summary>
    [XmlRoot("IWantToUpdateClearedTransactionsResponse", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToUpdateClearedTransactionsResponse : MsmqMessage<IWantToUpdateClearedTransactionsResponse>
    {
        public Int32 TotalTransactionsCount { get; set; }
        public Object ClearedTransactions { get; set; }
        public Guid SagaId { get; set; }
    }
}
