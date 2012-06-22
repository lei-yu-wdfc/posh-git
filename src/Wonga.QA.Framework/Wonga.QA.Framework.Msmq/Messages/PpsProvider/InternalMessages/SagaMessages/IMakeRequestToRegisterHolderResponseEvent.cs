using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToRegisterHolderMessageResponse </summary>
    [XmlRoot("IMakeRequestToRegisterHolderMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToRegisterHolderResponseEvent : MsmqMessage<IMakeRequestToRegisterHolderResponseEvent>
    {
        public ProcessStatusEnum HolderStatus { get; set; }
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
