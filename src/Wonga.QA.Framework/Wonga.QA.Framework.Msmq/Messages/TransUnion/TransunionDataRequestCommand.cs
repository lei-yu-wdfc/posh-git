using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.TransUnion
{
    /// <summary> Wonga.Transunion.InternalMessages.TransunionDataRequestMessage </summary>
    [XmlRoot("TransunionDataRequestMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class TransunionDataRequestCommand : MsmqMessage<TransunionDataRequestCommand>
    {
        public Object BureauEnquiry { get; set; }
        public DestinationEnum Destination { get; set; }
        public Guid SagaId { get; set; }
    }
}
