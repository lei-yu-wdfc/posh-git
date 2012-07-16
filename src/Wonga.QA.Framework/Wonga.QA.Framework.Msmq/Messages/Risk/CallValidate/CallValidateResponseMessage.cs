using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.CallValidate
{
    /// <summary> Wonga.Risk.CallValidate.CallValidateResponseMessage </summary>
    [XmlRoot("CallValidateResponseMessage", Namespace = "Wonga.Risk.CallValidate", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class CallValidateResponseMessage : MsmqMessage<CallValidateResponseMessage>
    {
        public Object Response { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
