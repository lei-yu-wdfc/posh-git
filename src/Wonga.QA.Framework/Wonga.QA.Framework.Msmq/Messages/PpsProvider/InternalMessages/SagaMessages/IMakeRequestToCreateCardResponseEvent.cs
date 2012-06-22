using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToCreateCardMessageResponse </summary>
    [XmlRoot("IMakeRequestToCreateCardMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToCreateCardResponseEvent : MsmqMessage<IMakeRequestToCreateCardResponseEvent>
    {
        public String AccountNumber { get; set; }
        public String CardPan { get; set; }
        public ProcessStatusEnum CardStatus { get; set; }
        public String ProviderId { get; set; }
        public String SerialNumber { get; set; }
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
