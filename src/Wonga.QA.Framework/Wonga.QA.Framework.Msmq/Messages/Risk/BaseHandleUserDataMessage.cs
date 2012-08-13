using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.BaseHandleUserDataMessage </summary>
    [XmlRoot("BaseHandleUserDataMessage", Namespace = "Wonga.Risk", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BaseHandleUserDataMessage : MsmqMessage<BaseHandleUserDataMessage>
    {
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
