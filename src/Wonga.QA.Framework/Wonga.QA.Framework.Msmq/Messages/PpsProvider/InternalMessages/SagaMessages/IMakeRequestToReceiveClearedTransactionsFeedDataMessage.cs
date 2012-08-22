using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToReceiveClearedTransactionsFeedDataMessage </summary>
    [XmlRoot("IMakeRequestToReceiveClearedTransactionsFeedDataMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PpsProvider.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMakeRequestToReceiveClearedTransactionsFeedDataMessage : MsmqMessage<IMakeRequestToReceiveClearedTransactionsFeedDataMessage>
    {
        public Guid SagaId { get; set; }
    }
}
