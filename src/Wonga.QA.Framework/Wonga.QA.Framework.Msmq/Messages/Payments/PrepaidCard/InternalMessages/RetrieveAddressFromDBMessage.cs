using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.RetrieveAddressFromDBMessage </summary>
    [XmlRoot("RetrieveAddressFromDBMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class RetrieveAddressFromDBMessage : MsmqMessage<RetrieveAddressFromDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
