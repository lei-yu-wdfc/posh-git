using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CallValidate.Handlers
{
    /// <summary> Wonga.CallValidate.Handlers.CallValidateInternalMessage </summary>
    [XmlRoot("CallValidateInternalMessage", Namespace = "Wonga.CallValidate.Handlers", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.CallValidate.Handlers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CallValidateInternalMessage : MsmqMessage<CallValidateInternalMessage>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
