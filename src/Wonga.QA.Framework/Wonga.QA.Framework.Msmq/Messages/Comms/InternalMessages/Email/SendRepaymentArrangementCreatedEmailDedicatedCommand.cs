using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendRepaymentArrangementCreatedEmailDedicatedMessage </summary>
    [XmlRoot("SendRepaymentArrangementCreatedEmailDedicatedMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendRepaymentArrangementCreatedEmailDedicatedCommand : MsmqMessage<SendRepaymentArrangementCreatedEmailDedicatedCommand>
    {
        public String Email { get; set; }
        public String FirstName { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
