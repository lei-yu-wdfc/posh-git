using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Email.SendApplicationReferredDeferredAcceptedTemplateC </summary>
    [XmlRoot("SendApplicationReferredDeferredAcceptedTemplateC", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "Wonga.Comms.InternalMessages.Email.BaseSimpleEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendApplicationReferredDeferredAcceptedTemplateCCommand : MsmqMessage<SendApplicationReferredDeferredAcceptedTemplateCCommand>
    {
        public String FirstName { get; set; }
        public String ApplicationDate { get; set; }
        public String LoanAmount { get; set; }
        public String Email { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
