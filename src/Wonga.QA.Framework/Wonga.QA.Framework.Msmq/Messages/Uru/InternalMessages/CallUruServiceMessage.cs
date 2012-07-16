using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Uru.InternalMessages
{
    /// <summary> Wonga.Uru.InternalMessages.CallUruServiceMessage </summary>
    [XmlRoot("CallUruServiceMessage", Namespace = "Wonga.Uru.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CallUruServiceMessage : MsmqMessage<CallUruServiceMessage>
    {
        public Guid SagaId { get; set; }
        public Guid AccountId { get; set; }
        public Object UruDataInput { get; set; }
    }
}
