using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToReceiveClearedTransactionsFeedDataMessageResponse </summary>
    [XmlRoot("IMakeRequestToReceiveClearedTransactionsFeedDataMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToReceiveClearedTransactionsFeedDataResponseEvent : MsmqMessage<IMakeRequestToReceiveClearedTransactionsFeedDataResponseEvent>
    {
        public Guid SagaId { get; set; }
    }
}
