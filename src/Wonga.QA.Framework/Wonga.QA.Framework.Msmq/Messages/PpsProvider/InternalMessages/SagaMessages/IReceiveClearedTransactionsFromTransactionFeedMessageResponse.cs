using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IReceiveClearedTransactionsFromTransactionFeedMessageResponse </summary>
    [XmlRoot("IReceiveClearedTransactionsFromTransactionFeedMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PpsProvider.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IReceiveClearedTransactionsFromTransactionFeedMessageResponse : MsmqMessage<IReceiveClearedTransactionsFromTransactionFeedMessageResponse>
    {
        public Int32 TotalTransactionsCount { get; set; }
        public Object Transactions { get; set; }
        public Guid SagaId { get; set; }
    }
}
