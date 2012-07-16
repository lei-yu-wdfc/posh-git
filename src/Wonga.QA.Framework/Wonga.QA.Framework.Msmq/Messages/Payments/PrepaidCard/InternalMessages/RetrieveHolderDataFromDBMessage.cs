using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.RetrieveHolderDataFromDBMessage </summary>
    [XmlRoot("RetrieveHolderDataFromDBMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class RetrieveHolderDataFromDBMessage : MsmqMessage<RetrieveHolderDataFromDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
