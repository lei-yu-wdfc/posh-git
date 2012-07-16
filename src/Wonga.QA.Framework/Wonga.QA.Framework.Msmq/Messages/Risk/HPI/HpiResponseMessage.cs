using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.HPI
{
    /// <summary> Wonga.Risk.HPI.HpiResponseMessage </summary>
    [XmlRoot("HpiResponseMessage", Namespace = "Wonga.Risk.HPI", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class HpiResponseMessage : MsmqMessage<HpiResponseMessage>
    {
        public Object VehicleReport { get; set; }
        public String[] Errors { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
