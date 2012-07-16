using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Transunion;

namespace Wonga.QA.Framework.Msmq.Messages.Transunion.InternalMessages
{
    /// <summary> Wonga.Transunion.InternalMessages.TransunionDataRequestMessage </summary>
    [XmlRoot("TransunionDataRequestMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class TransunionDataRequestMessage : MsmqMessage<TransunionDataRequestMessage>
    {
        public Object BureauEnquiry { get; set; }
        public DestinationEnum Destination { get; set; }
        public Guid SagaId { get; set; }
    }
}
