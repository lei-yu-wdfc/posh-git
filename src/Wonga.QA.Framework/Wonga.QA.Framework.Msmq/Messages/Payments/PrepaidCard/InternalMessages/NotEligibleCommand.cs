using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.NotEligibleMessage </summary>
    [XmlRoot("NotEligibleMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class NotEligibleCommand : MsmqMessage<NotEligibleCommand>
    {
        public Guid SagaId { get; set; }
    }
}
