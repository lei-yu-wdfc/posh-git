using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Email
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendApplicationReferredDeferredAcceptedTemplateD </summary>
    [XmlRoot("SendApplicationReferredDeferredAcceptedTemplateD", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.BaseSimpleEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Email, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendApplicationReferredDeferredAcceptedTemplateD : MsmqMessage<SendApplicationReferredDeferredAcceptedTemplateD>
    {
        public String FirstName { get; set; }
        public String MaxLoan { get; set; }
        public String LoanAmount { get; set; }
        public String Email { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
