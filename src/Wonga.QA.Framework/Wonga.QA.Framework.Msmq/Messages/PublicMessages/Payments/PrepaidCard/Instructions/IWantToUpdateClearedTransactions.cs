using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateClearedTransactions </summary>
    [XmlRoot("IWantToUpdateClearedTransactions", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToUpdateClearedTransactions : MsmqMessage<IWantToUpdateClearedTransactions>
    {
        public Guid SagaId { get; set; }
    }
}
