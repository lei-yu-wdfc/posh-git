using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendRepaymentArrangementCancelledDedicatedMessage </summary>
    [XmlRoot("SendRepaymentArrangementCancelledDedicatedMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendRepaymentArrangementCancelledDedicatedMessage : MsmqMessage<SendRepaymentArrangementCancelledDedicatedMessage>
    {
        public String FirstName { get; set; }
        public String Email { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
