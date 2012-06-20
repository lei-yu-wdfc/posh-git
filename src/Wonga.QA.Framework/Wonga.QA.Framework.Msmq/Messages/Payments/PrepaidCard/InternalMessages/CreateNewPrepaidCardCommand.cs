using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.PrepaidCard.InternalMessages;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.CreateNewPrepaidCardMessage </summary>
    [XmlRoot("CreateNewPrepaidCardMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CreateNewPrepaidCardCommand : MsmqMessage<CreateNewPrepaidCardCommand>
    {
        public CardEnum CardType { get; set; }
        public Guid CustomerExternalId { get; set; }
        public Guid SagaId { get; set; }
    }
}
