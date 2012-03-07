using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Email.BaseRepaymentArrangementEmailMessage </summary>
    [XmlRoot("BaseRepaymentArrangementEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.BaseSimpleEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class BaseRepaymentArrangementEmailCommand : MsmqMessage<BaseRepaymentArrangementEmailCommand>
    {
        public String FirstName { get; set; }
        public Object Arrangements { get; set; }
        public String Email { get; set; }
        public Guid SagaId { get; set; }
    }
}
