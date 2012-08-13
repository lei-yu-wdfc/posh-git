using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Equifax.Handlers
{
    /// <summary> Wonga.Equifax.Handlers.EquifaxDataRequestMessage </summary>
    [XmlRoot("EquifaxDataRequestMessage", Namespace = "Wonga.Equifax.Handlers", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Equifax.Handlers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class EquifaxDataRequestMessage : MsmqMessage<EquifaxDataRequestMessage>
    {
        public Object Request { get; set; }
        public Guid SagaId { get; set; }
    }
}
