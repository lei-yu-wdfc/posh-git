using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Experian.Handlers
{
    /// <summary> Wonga.Experian.Handlers.ExperianInternalCardMessage </summary>
    [XmlRoot("ExperianInternalCardMessage", Namespace = "Wonga.Experian.Handlers", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Experian.Handlers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExperianInternalCardMessage : MsmqMessage<ExperianInternalCardMessage>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
