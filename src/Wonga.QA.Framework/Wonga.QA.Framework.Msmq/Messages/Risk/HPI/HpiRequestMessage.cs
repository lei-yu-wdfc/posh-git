using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.HPI
{
    /// <summary> Wonga.Risk.HPI.HpiRequestMessage </summary>
    [XmlRoot("HpiRequestMessage", Namespace = "Wonga.Risk.HPI", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class HpiRequestMessage : MsmqMessage<HpiRequestMessage>
    {
        public String VehicleRegistration { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
