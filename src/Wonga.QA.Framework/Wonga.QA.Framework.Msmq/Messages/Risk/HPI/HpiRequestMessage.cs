using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.HPI
{
    /// <summary> Wonga.Risk.HPI.HpiRequestMessage </summary>
    [XmlRoot("HpiRequestMessage", Namespace = "Wonga.Risk.HPI", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HpiRequestMessage : MsmqMessage<HpiRequestMessage>
    {
        public String VehicleRegistration { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
