using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.InternalMessages.Za
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Za.BaseSaveEmailFileCompleteMessage </summary>
    [XmlRoot("BaseSaveEmailFileCompleteMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Za", DataType = "Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.DocumentGeneration.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BaseSaveEmailFileCompleteMessage : MsmqMessage<BaseSaveEmailFileCompleteMessage>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
