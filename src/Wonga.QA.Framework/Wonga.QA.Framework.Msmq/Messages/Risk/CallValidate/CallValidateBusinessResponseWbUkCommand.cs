using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.CallValidate
{
    /// <summary> Wonga.Risk.CallValidate.CallValidateBusinessResponseMessage </summary>
    [XmlRoot("CallValidateBusinessResponseMessage", Namespace = "Wonga.Risk.CallValidate", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class CallValidateBusinessResponseWbUkCommand : MsmqMessage<CallValidateBusinessResponseWbUkCommand>
    {
        public Object Response { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
