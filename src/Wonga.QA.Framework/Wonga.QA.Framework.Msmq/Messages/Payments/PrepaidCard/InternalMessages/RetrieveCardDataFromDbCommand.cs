using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.RetrieveCardDataFromDBMessage </summary>
    [XmlRoot("RetrieveCardDataFromDBMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class RetrieveCardDataFromDbCommand : MsmqMessage<RetrieveCardDataFromDbCommand>
    {
        public Guid SagaId { get; set; }
    }
}
