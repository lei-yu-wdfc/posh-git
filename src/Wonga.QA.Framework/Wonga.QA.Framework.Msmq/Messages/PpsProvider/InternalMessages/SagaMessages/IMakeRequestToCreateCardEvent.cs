using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PpsProvider.InternalMessages.SagaMessages;
using Wonga.QA.Framework.Msmq.Enums.Payments.PrepaidCard.InternalMessages;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToCreateCardMessage </summary>
    [XmlRoot("IMakeRequestToCreateCardMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToCreateCardEvent : MsmqMessage<IMakeRequestToCreateCardEvent>
    {
        public ProcessStatusEnum HolderStatus { get; set; }
        public Guid ExternalId { get; set; }
        public Guid CustomerExternalId { get; set; }
        public ProcessStatusEnum CardStatus { get; set; }
        public CardEnum CardType { get; set; }
        public Guid CardExternalId { get; set; }
        public Guid SagaId { get; set; }
    }
}
