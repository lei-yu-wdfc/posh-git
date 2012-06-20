using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.CallValidate.Handlers
{
    /// <summary> Wonga.CallValidate.Handlers.CallValidateInternalMessage </summary>
    [XmlRoot("CallValidateInternalMessage", Namespace = "Wonga.CallValidate.Handlers", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CallValidateInternalCommand : MsmqMessage<CallValidateInternalCommand>
    {
        public Guid SagaId { get; set; }
        public Object Request { get; set; }
    }
}
