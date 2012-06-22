using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendRepaymentArrangementEarlyPartiallyRepaidEmailMessage </summary>
    [XmlRoot("SendRepaymentArrangementEarlyPartiallyRepaidEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.BaseRepaymentArrangementEmailMessage,Wonga.Comms.InternalMessages.Email.BaseSimpleEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendRepaymentArrangementEarlyPartiallyRepaidEmailCommand : MsmqMessage<SendRepaymentArrangementEarlyPartiallyRepaidEmailCommand>
    {
        public String FirstName { get; set; }
        public Object Arrangements { get; set; }
        public String Email { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}