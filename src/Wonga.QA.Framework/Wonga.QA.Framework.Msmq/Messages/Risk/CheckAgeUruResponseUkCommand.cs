using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("CheckAgeUruResponseMessage", Namespace = "Wonga.Risk.URU", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public class CheckAgeUruResponseUkCommand : MsmqMessage<CheckAgeUruResponseUkCommand>
    {
        public Boolean AgeUnder18 { get; set; }
        public Boolean AgeConfirmed { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
