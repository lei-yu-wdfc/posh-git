using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IReceiveClearedTransactionsFromTransactionFeedMessage </summary>
    [XmlRoot("IReceiveClearedTransactionsFromTransactionFeedMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IReceiveClearedTransactionsFromTransactionFeedEvent : MsmqMessage<IReceiveClearedTransactionsFromTransactionFeedEvent>
    {
        public Guid SagaId { get; set; }
    }
}
