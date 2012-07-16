using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitUidAnswersMessage </summary>
    [XmlRoot("SubmitUidAnswersMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseHandleUserDataMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SubmitUidAnswers : MsmqMessage<SubmitUidAnswers>
    {
        public Object Answers { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
