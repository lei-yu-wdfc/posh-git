using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.HPI
{
    /// <summary> Wonga.Risk.HPI.HpiResponseMessage </summary>
    [XmlRoot("HpiResponseMessage", Namespace = "Wonga.Risk.HPI", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HpiResponseMessage : MsmqMessage<HpiResponseMessage>
    {
        public Object VehicleReport { get; set; }
        public String[] Errors { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
