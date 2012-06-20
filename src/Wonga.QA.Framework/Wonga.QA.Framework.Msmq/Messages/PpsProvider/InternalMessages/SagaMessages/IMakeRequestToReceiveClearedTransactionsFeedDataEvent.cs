using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToReceiveClearedTransactionsFeedDataMessage </summary>
    [XmlRoot("IMakeRequestToReceiveClearedTransactionsFeedDataMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToReceiveClearedTransactionsFeedDataEvent : MsmqMessage<IMakeRequestToReceiveClearedTransactionsFeedDataEvent>
    {
        public Guid SagaId { get; set; }
    }
}
