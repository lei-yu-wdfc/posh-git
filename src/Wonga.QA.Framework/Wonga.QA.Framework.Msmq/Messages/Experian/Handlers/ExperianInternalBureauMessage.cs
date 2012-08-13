using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Experian.Handlers
{
    /// <summary> Wonga.Experian.Handlers.ExperianInternalBureauMessage </summary>
    [XmlRoot("ExperianInternalBureauMessage", Namespace = "Wonga.Experian.Handlers", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Experian.Handlers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExperianInternalBureauMessage : MsmqMessage<ExperianInternalBureauMessage>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
