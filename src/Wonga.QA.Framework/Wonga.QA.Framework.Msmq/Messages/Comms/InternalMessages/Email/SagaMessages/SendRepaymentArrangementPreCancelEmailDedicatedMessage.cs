using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email.SagaMessages
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SagaMessages.SendRepaymentArrangementPreCancelEmailDedicatedMessage </summary>
    [XmlRoot("SendRepaymentArrangementPreCancelEmailDedicatedMessage", Namespace = "Wonga.Comms.InternalMessages.Email.SagaMessages", DataType = "")]
    public partial class SendRepaymentArrangementPreCancelEmailDedicatedMessage : MsmqMessage<SendRepaymentArrangementPreCancelEmailDedicatedMessage>
    {
        public String FirstName { get; set; }
        public String Email { get; set; }
        public DateTime GracePeriodDeadline { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Guid SagaId { get; set; }
    }
}
