using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendRepaymentArrangementCreatedEmailDedicatedMessage </summary>
    [XmlRoot("SendRepaymentArrangementCreatedEmailDedicatedMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendRepaymentArrangementCreatedEmailDedicatedMessage : MsmqMessage<SendRepaymentArrangementCreatedEmailDedicatedMessage>
    {
        public String Email { get; set; }
        public String FirstName { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}
