using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SagaMessages.UpdateSagaTimeoutMessage </summary>
    [XmlRoot("UpdateSagaTimeoutMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "Wonga.Payments.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateSagaTimeoutMessage : MsmqMessage<UpdateSagaTimeoutMessage>
    {
        public DateTime Expires { get; set; }
        public Object State { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
