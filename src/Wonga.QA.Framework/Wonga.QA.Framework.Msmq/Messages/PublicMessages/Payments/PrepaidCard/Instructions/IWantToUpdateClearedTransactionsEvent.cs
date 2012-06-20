using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateClearedTransactions </summary>
    [XmlRoot("IWantToUpdateClearedTransactions", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToUpdateClearedTransactionsEvent : MsmqMessage<IWantToUpdateClearedTransactionsEvent>
    {
        public String OriginalMessageId { get; set; }
        public Guid SagaId { get; set; }
    }
}
