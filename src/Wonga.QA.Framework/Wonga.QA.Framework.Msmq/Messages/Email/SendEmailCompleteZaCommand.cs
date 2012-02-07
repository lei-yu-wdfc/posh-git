using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Email
{
    [XmlRoot("SendEmailCompleteMessage", Namespace = "Wonga.Email.InternalMessages.Za.SagaMessages", DataType = "Wonga.Email.InternalMessages.Za.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendEmailCompleteZaCommand : MsmqMessage<SendEmailCompleteZaCommand>
    {
        public Guid SagaId { get; set; }
    }
}
